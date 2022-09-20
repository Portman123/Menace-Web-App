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

        private Turn PlayMenaceTurn(GameHistory game, PlayerMenace aiPlayer, BoardPosition initialBoardPosition, string playerSymbol, int turnNumber)
        {
            initialBoardPosition = _context.BoardPosition.GetOrAddIfNotExists(initialBoardPosition, b => b.BoardPositionId == initialBoardPosition.BoardPositionId);

            var aiTurn = aiPlayer.PlayTurn(initialBoardPosition, MapPlayerLetterToPlayerNumber(playerSymbol), turnNumber);

            aiTurn.After = _context.BoardPosition.GetOrAddIfNotExists(aiTurn.After, b => b.BoardPositionId == aiTurn.After.BoardPositionId);

            _context.Turn.Add(aiTurn);

            game.AddMove(aiTurn);

            var matchbox = aiPlayer.MenaceEngine.Matchboxes.Single(m => m.BoardPosition.BoardPositionId == initialBoardPosition.BoardPositionId);

            if (_context.Matchbox.AddIfNotExists(matchbox, m => m.Id == matchbox.Id))
            {
                _context.Entry(matchbox).Reference(m => m.BoardPosition).IsModified = false;

                foreach (var bead in matchbox.Beads)
                {
                    _context.Bead.Add(bead);
                }
            }

            return aiTurn;
        }

        private Turn RecordHumanTurn(GameHistory game, Player humanPlayer, BoardPosition boardBeforeInput, BoardPosition boardAfterInput)
        {
            var humanMove = BoardPosition.GetMove(boardBeforeInput, boardAfterInput);

            var humanTurn = new Turn(humanPlayer, boardBeforeInput, boardAfterInput, humanMove.X, humanMove.Y, boardBeforeInput.TurnNumber);

            _context.Turn.Add(humanTurn);

            game.AddMove(humanTurn);

            return humanTurn;
        }

        [HttpGet]
        public IActionResult Build(GameCreate createGameInput)
        {
            // Set-up game
            Player player1;
            Player player2;

            if (createGameInput.Type == GameType.MenaceP1)
            {
                player1 = GetOrCreatePlayerMenace($"Menace as P1 #{_context.Player.Count()}");
                player2 = GetOrPlayerHuman($"Human #{_context.Player.Count() + 1}");
            }
            else if (createGameInput.Type == GameType.MenaceP2)
            {
                player1 = GetOrPlayerHuman($"Human #{_context.Player.Count()}");
                player2 = GetOrCreatePlayerMenace($"Menace as P2 #{_context.Player.Count() + 1}");
            }
            else { throw new Exception("Invalid input when choosing if Menace is P1 or P2"); }

            var newGame = new GameHistory(player1, player2);

            _context.Add(newGame);

            var boardPosition = new BoardPosition();

            var playerSymbol = "X";

            if (player1 is PlayerMenace)
            {
                PlayMenaceTurn(newGame, player1 as PlayerMenace, boardPosition, playerSymbol, 1);

                boardPosition = newGame.Turns.Last().After;

                playerSymbol = "O";
            }

            _context.SaveChanges();

            // Set-up UI
            ModelState.Clear();

            var gameState = new GamePlayState
            {
                BoardBeforeInput = GamePlayState.WrapBoard(boardPosition.BoardPositionId),
                GameHistoryId = newGame.Id,
                IsGameActive = true,
                CurrentPlayerSymbol = playerSymbol
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
                // --------------------------------------------
                
                // boards from before and after user input
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

                var humanPlayer = game.P1 is PlayerMenace ? game.P2 : game.P1;

                var nonHumanPlayer = game.P1 is PlayerMenace ? game.P1 : game.P2;

                var aiPlayer = PlayerFactory.GetPlayer(_context, nonHumanPlayer.Id, PlayerType.AIMenace) as PlayerMenace;

                var humanSymbol = humanPlayer == game.P1 ? "X" : "O";

                var aiSymbol = nonHumanPlayer == game.P1 ? "X" : "O";

                // Human player's turn
                var humanTurn = RecordHumanTurn(game, humanPlayer, boardBeforeInput, boardAfterInput);

                // Check win
                if (humanTurn.After.IsGameOver)
                {
                    return HandleEndOfGame(game, humanTurn, humanSymbol);
                }

                // AI player's turn
                var aiTurn = PlayMenaceTurn(game, aiPlayer, boardAfterInput, aiSymbol, boardAfterInput.TurnNumber);

                // Check win
                if (aiTurn.After.IsGameOver)
                {
                    return HandleEndOfGame(game, aiTurn, aiSymbol);
                }
                _context.SaveChanges();

                // Reload UI with new game state
                ModelState.Clear();

                var newGameState = new GamePlayState
                {
                    BoardBeforeInput = GamePlayState.WrapBoard(aiTurn.After.BoardPositionId),
                    GameHistoryId = gameState.GameHistoryId,
                    IsGameActive = true,
                    CurrentPlayerSymbol = humanSymbol
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

        private Player GetOrPlayerHuman(string name)
        {
            var player = _context.Player.Where(p => p.Name == name).FirstOrDefault();

            if (player == null)
            {
                player = new PlayerHumanOnWeb(name);

                _context.Player.Add(player);
            }

            return player;
        }

        private Player GetOrCreatePlayerMenace(string name)
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
