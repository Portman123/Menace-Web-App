using Menace.Services;
using Menace.ViewModels;
using MenaceData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noughts_and_Crosses;

namespace Menace.Controllers
{
    public class BuildMenaceController : Controller
    {
        private readonly MenaceContext _context;
        private Game game;
        //private PlayerMenace menace;
        //private PlayerHumanOnWeb human;
        //private BoardPosition boardPosition;

        public BuildMenaceController(MenaceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult BuildSetup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BuildSetup(GameCreate createGameInput)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Build), createGameInput);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Build(GameCreate createGameInput)
        {
            // Set-up game
            Player player1 = null;
            Player player2 = null;

            if (createGameInput.Type == GameType.MenaceP1)
            {
                player1 = GetPlayerMenace($"MenaceP1 {_context.Player.Count()}");
                player2 = GetPlayerHuman($"Human {_context.Player.Count() + 1}");
            }
            else if (createGameInput.Type == GameType.MenaceP2)
            {
                player1 = GetPlayerHuman($"Human {_context.Player.Count()}");
                player2 = GetPlayerMenace($"MenaceP2 {_context.Player.Count() + 1}");
            }
            else { throw new Exception("Invalid input when choosing if Menace is P1 or P2"); }

            var newGame = new GameHistory(player1, player2);

            _context.Add(newGame);

            _context.SaveChanges();

            // Set-up UI
            ModelState.Clear();

            var gameState = new GamePlayState
            {
                BoardBeforeInput = GamePlayState.WrapBoard(BoardPosition.EmptyBoardPostion),
                GameHistoryId = newGame.Id,
                IsGameActive = true
            };

            return View(gameState);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Build([Bind("BoardBeforeInput, BoardAfterInput, CurrentPlayerSymbol, GameHistoryId")] GamePlayState gameState)
        {
            if (ModelState.IsValid)
            {
                // Load state from inputs
                // Load boards from before and after user input
                var boardBeforeInput = new BoardPosition
                {
                    BoardPositionId = GamePlayState.UnwrapBoard(gameState.BoardBeforeInput)
                };

                boardBeforeInput = _context.BoardPosition.GetOrAddIfNotExists(boardBeforeInput, b => b.BoardPositionId == boardBeforeInput.BoardPositionId);

                var boardAfterInput = new BoardPosition
                {
                    BoardPositionId = GamePlayState.UnwrapBoard(gameState.BoardAfterInput)
                };

                boardAfterInput = _context.BoardPosition.GetOrAddIfNotExists(boardAfterInput, b => b.BoardPositionId == boardAfterInput.BoardPositionId);

                // Load game history with players
                var game = _context.GameHistory.Where(g => g.Id == gameState.GameHistoryId)
                    .Include(g => g.P1)
                    .Include(g => g.P2)
                    .Include(g => g.Turns)
                    .Single();

                var humanPlayer = game.P1;

                //var humanPlayer = PlayerFactory.GetPlayer(_context, game.P1.Id, PlayerType.Human);

                var aiPlayer = PlayerFactory.GetPlayer(_context, game.P2.Id, PlayerType.AIMenace) as PlayerMenace;

                game.P2 = aiPlayer;

                // Add turn just played to Game History
                var humanMove = BoardPosition.GetMove(boardBeforeInput, boardAfterInput);

                var humanTurn = new Turn(humanPlayer, boardBeforeInput, boardAfterInput, humanMove.X, humanMove.Y, boardBeforeInput.TurnNumber);

                _context.Turn.Add(humanTurn);

                game.AddMove(humanTurn);

                // Did human player make winning move or last move (draw)?
                if (humanTurn.After.IsGameOver)
                {
                    return HandleEndOfGame(game, humanTurn, "X");
                }

                // Make AI play its turn
                var aiTurn = aiPlayer.PlayTurn(boardAfterInput, MapPlayerLetterToPlayerNumber(gameState.CurrentPlayerSymbol), boardAfterInput.TurnNumber);

                aiTurn.After = _context.BoardPosition.GetOrAddIfNotExists(aiTurn.After, b => b.BoardPositionId == aiTurn.After.BoardPositionId);

                _context.Turn.Add(aiTurn);

                game.AddMove(aiTurn);

                var matchbox = aiPlayer.MenaceEngine.Matchboxes.Single(m => m.BoardPosition.BoardPositionId == boardAfterInput.BoardPositionId);

                if (_context.Matchbox.AddIfNotExists(matchbox, m => m.Id == matchbox.Id))
                {
                    _context.Entry(matchbox).Reference(m => m.BoardPosition).IsModified = false;

                    foreach (var bead in matchbox.Beads)
                    {
                        _context.Bead.Add(bead);
                    }
                }

                // Did AI player make winning move or last move (draw)?
                if (aiTurn.After.IsGameOver)
                {
                    return HandleEndOfGame(game, aiTurn, "O");
                }

                _context.SaveChanges();

                // Reload UI with new game state
                ModelState.Clear();

                var newGameState = new GamePlayState
                {
                    BoardBeforeInput = GamePlayState.WrapBoard(aiTurn.After.BoardPositionId),
                    GameHistoryId = gameState.GameHistoryId,
                    IsGameActive = true
                };

                return View(newGameState);
            }

            return View(gameState);
        }




        // Service Methods
        //-----------------
        private int MapPlayerLetterToPlayerNumber(string letter) => letter == "X" ? -1 : 1;

        private IActionResult HandleEndOfGame(GameHistory game, Turn lastTurn, string currentPlayerSymbol)
        {
            // Record final state
            game.IsGameFinished = true;

            if (lastTurn.After.IsWinningPosition)
            {
                game.Winner = lastTurn.TurnPlayer;
            }

            _context.SaveChanges();

            // Display end of game UI
            ModelState.Clear();

            var finalState = new GamePlayState
            {
                BoardBeforeInput = GamePlayState.WrapBoard(lastTurn.After.BoardPositionId),
                CurrentPlayerSymbol = currentPlayerSymbol,
                IsGameActive = false
            };

            return View(finalState);
        }

        private Player GetPlayerHuman(string name)
        {
            var player = _context.Player.Where(p => p.Name == name).FirstOrDefault();

            if (player == null)
            {
                player = new PlayerHumanOnWeb(name);

                _context.Player.Add(player);
            }

            return player;
        }

        private Player GetPlayerMenace(string name)
        {
            var player = _context.Player.Where(p => p.Name == name).FirstOrDefault();

            if (player == null)
            {
                var ai = new AIMenace();
                player = new PlayerMenace(ai, name);

                _context.AIMenace.Add(ai);
                _context.Player.Add(player);

            }

            return player;
        }

    }
}
