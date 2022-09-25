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
                if (createGameInput.GameType == GameType.MenaceP1)
                {
                    createGameInput.Player1Id = GetOrCreatePlayerMenace($"Menace {_context.PlayerMenace.Count()}", createGameInput.RewardFunctionType).Id;
                    createGameInput.Player2Id = GetOrPlayerHuman(Player.HumanPlayerName).Id;
                }
                else if (createGameInput.GameType == GameType.MenaceP2)
                {
                    createGameInput.Player1Id = GetOrPlayerHuman(Player.HumanPlayerName).Id;
                    createGameInput.Player2Id = GetOrCreatePlayerMenace($"Menace {_context.PlayerMenace.Count()}", createGameInput.RewardFunctionType).Id;
                }
                else { throw new Exception("Invalid input when choosing if Menace is P1 or P2"); }

                _context.SaveChanges();

                return RedirectToAction(nameof(Build), createGameInput);
            }
            return View();
        }

        private Turn PlayMenaceTurn(GameHistory game, PlayerMenace aiPlayer, BoardPosition initialBoardPosition, string playerSymbol, int turnNumber)
        {
            initialBoardPosition = _context.BoardPosition.GetOrAddIfNotExists(initialBoardPosition, b => b.BoardPositionId == initialBoardPosition.BoardPositionId);

            var aiTurn = aiPlayer.PlayTurn(initialBoardPosition, GameSymbol.MapSymbolToInt(playerSymbol), turnNumber);

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

            if (createGameInput.GameType == GameType.MenaceP1)
            {
                player1 = PlayerFactory.GetPlayer(_context, createGameInput.Player1Id, PlayerType.AIMenace);
                player2 = PlayerFactory.GetPlayer(_context, createGameInput.Player2Id, PlayerType.Human);

            }
            else if (createGameInput.GameType == GameType.MenaceP2)
            {
                player1 = PlayerFactory.GetPlayer(_context, createGameInput.Player1Id, PlayerType.Human);
                player2 = PlayerFactory.GetPlayer(_context, createGameInput.Player2Id, PlayerType.AIMenace);
            }
            else { throw new Exception("Invalid input when choosing if Menace is P1 or P2"); }

            var aiPlayer = player1 as PlayerMenace ?? player2 as PlayerMenace;

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
                CurrentPlayerSymbol = playerSymbol,
                GameType = createGameInput.GameType,
                Player1Id = player1.Id,
                Player2Id = player2.Id,
                Beads = aiPlayer.MenaceEngine.MatchboxByBoardPos(boardPosition)?.Beads,
                Matchbox = aiPlayer.MenaceEngine.MatchboxByBoardPos(boardPosition),
                MenaceName = aiPlayer.Name
            };

            return View(gameState);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Build([Bind("BoardBeforeInput, BoardAfterInput, CurrentPlayerSymbol, GameHistoryId, GameType")] GamePlayState gameState)
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
                    return HandleEndOfGame(game, humanTurn, humanSymbol, aiPlayer, gameState.GameType);
                }

                // AI player's turn
                var aiTurn = PlayMenaceTurn(game, aiPlayer, boardAfterInput, aiSymbol, boardAfterInput.TurnNumber);

                // Check win
                if (aiTurn.After.IsGameOver)
                {
                    return HandleEndOfGame(game, aiTurn, aiSymbol, aiPlayer, gameState.GameType);
                }
                _context.SaveChanges();

                // Reload UI with new game state
                ModelState.Clear();

                var matchbox = aiPlayer.MenaceEngine.MatchboxByBoardPos(aiTurn.Before);

                var newGameState = new GamePlayState
                {
                    BoardBeforeInput = GamePlayState.WrapBoard(aiTurn.After.BoardPositionId),
                    GameHistoryId = gameState.GameHistoryId,
                    IsGameActive = true,
                    CurrentPlayerSymbol = humanSymbol,
                    GameType = gameState.GameType,
                    Player1Id = humanPlayer.Id,
                    Player2Id = aiPlayer.Id,
                    Beads = matchbox?.Beads,
                    Matchbox = matchbox,
                    MenaceName = aiPlayer.Name
                };
                return View(newGameState);
            }

            return View(gameState);
        }




        // Service Methods
        //-----------------
        private IActionResult HandleEndOfGame(GameHistory game, Turn lastTurn, string currentPlayerSymbol, PlayerMenace aiPlayer, GameType gameType)
        {
            // Record final state
            if (lastTurn.After.IsWinningPosition)
            {
                game.Winner.Wins++;
                // Convoluted but meh
                if (game.P1 == game.Winner)
                {
                    game.P2.Losses++;
                }
                else
                {
                    game.P1.Losses++;
                }
                // Reinforce aiPlayer
                if (aiPlayer != null)
                {
                    ReinforcementIncremental.Reinforce(game, aiPlayer);
                }
            }
            else if (lastTurn.After.IsBoardFull)
            {
                game.P1.Draws++;
                game.P2.Draws++;
                // Reinforce aiPlayer
                if (aiPlayer != null)
                {
                    ReinforcementIncremental.Reinforce(game, aiPlayer);
                }
            }

            _context.SaveChanges();

            // Display end of game UI
            ModelState.Clear();

            var finalState = new GamePlayState
            {
                BoardBeforeInput = GamePlayState.WrapBoard(lastTurn.After.BoardPositionId),
                CurrentPlayerSymbol = currentPlayerSymbol,
                IsGameActive = false,
                GameType = gameType == GameType.MenaceP1 ? GameType.MenaceP2 : GameType.MenaceP1,
                Player1Id = game.P2.Id,
                Player2Id = game.P1.Id,
                Beads = aiPlayer.MenaceEngine.MatchboxByBoardPos(lastTurn.Before)?.Beads,
                Matchbox = aiPlayer.MenaceEngine.MatchboxByBoardPos(lastTurn.Before),
                MenaceName = aiPlayer.Name
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

        private Player GetOrCreatePlayerMenace(string name, ReinforcementRewardFunction.RewardFunctionType rewardFunctionType)
        {
            var player = _context.Player.Where(p => p.Name == name).FirstOrDefault();

            if (player == null)
            {
                var ai = new AIMenace();
                player = new PlayerMenace(ai, name, rewardFunctionType);

                _context.AIMenace.Add(ai);
                _context.Player.Add(player);

                return player;
            }

            //return player;
            return PlayerFactory.GetPlayer(_context, _context.Player.Where(p => p.Name == name).FirstOrDefault().Id, PlayerType.AIMenace);
        }



        [HttpGet]
        public IActionResult TrainOptimal(GameCreate createGameInput)
        {
            if (createGameInput.GameType == GameType.MenaceP1)
            {
                TrainingService.TrainOptimal(_context, createGameInput.Player1Id);
            }
            else if (createGameInput.GameType == GameType.MenaceP2)
            {
                TrainingService.TrainOptimal(_context, createGameInput.Player2Id);
            }
            else
            {
                throw new Exception("Invalid input when choosing if Menace is P1 or P2");
            }

            return RedirectToAction(nameof(Build), createGameInput);
        }

        [HttpGet]
        public IActionResult TrainRandom(GameCreate createGameInput)
        {
            if (createGameInput.GameType == GameType.MenaceP1)
            {
                TrainingService.TrainRandom(_context, createGameInput.Player1Id);
            }
            else if (createGameInput.GameType == GameType.MenaceP2)
            {
                TrainingService.TrainRandom(_context, createGameInput.Player2Id);
            }
            else
            {
                throw new Exception("Invalid input when choosing if Menace is P1 or P2");
            }

            return RedirectToAction(nameof(Build), createGameInput);
        }

        //[HttpGet]
        //public IActionResult TrainMenace(GameCreate createGameInput)
        //{
        //    // Setup players
        //    PlayerMenace playerMenace;
        //    PlayerMenace playerMenaceTrainer;

        //    // Get Menace Player
        //    if (createGameInput.Type == GameType.MenaceP1)
        //    {
        //        // Get user's player Menace
        //        playerMenace = PlayerFactory.GetPlayer(_context, createGameInput.Player1Id, PlayerType.AIMenace) as PlayerMenace;
        //    }
        //    else if (createGameInput.Type == GameType.MenaceP2)
        //    {
        //        // Get user's player Menace
        //        playerMenace = PlayerFactory.GetPlayer(_context, createGameInput.Player2Id, PlayerType.AIMenace) as PlayerMenace;
        //    }
        //    else { throw new Exception("Invalid input when choosing if Menace is P1 or P2"); }


        //    // Get Menace Trainer (Pick random Menace from database)
        //    var MenacePlayers = _context.Player.Where(p => p is PlayerMenace && p.Id != playerMenace.Id).ToList();
        //    var temp = MenacePlayers[RandomNumberGenerator.Next(MenacePlayers.Count)];
        //    playerMenaceTrainer = PlayerFactory.GetPlayer(_context, temp.Id, PlayerType.AIMenace) as PlayerMenace;

        //    // if there are no other menaces for menace to train against then don't try to train...
        //    if (playerMenaceTrainer == null) return RedirectToAction(nameof(Build), createGameInput);

        //    // train Menace
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        var game1 = new Game(playerMenace, playerMenaceTrainer);
        //        var game2 = new Game(playerMenaceTrainer, playerMenace);

        //        game1.Train();
        //        game2.Train();
        //    }

        //    // Add new matchboxes and beads.
        //    foreach (Matchbox matchbox in playerMenace.MenaceEngine.Matchboxes)
        //    {
        //        matchbox.BoardPosition = _context.BoardPosition.GetOrAddIfNotExists(matchbox.BoardPosition, b => b.BoardPositionId == matchbox.BoardPosition.BoardPositionId);

        //        if (_context.Matchbox.AddIfNotExists(matchbox, m => m.Id == matchbox.Id))
        //        {
        //            _context.Entry(matchbox).Reference(m => m.BoardPosition).IsModified = false;

        //            foreach (var bead in matchbox.Beads)
        //            {
        //                _context.Bead.Add(bead);
        //            }
        //        }
        //    }

        //    // Add new matchboxes and beads.
        //    foreach (Matchbox matchbox in playerMenaceTrainer.MenaceEngine.Matchboxes)
        //    {
        //        matchbox.BoardPosition = _context.BoardPosition.GetOrAddIfNotExists(matchbox.BoardPosition, b => b.BoardPositionId == matchbox.BoardPosition.BoardPositionId);

        //        if (_context.Matchbox.AddIfNotExists(matchbox, m => m.Id == matchbox.Id))
        //        {
        //            _context.Entry(matchbox).Reference(m => m.BoardPosition).IsModified = false;

        //            foreach (var bead in matchbox.Beads)
        //            {
        //                _context.Bead.Add(bead);
        //            }
        //        }
        //    }

        //    // Don't add progress of the random Menace??

        //    _context.SaveChanges();

        //    // Display end of game UI
        //    ModelState.Clear();

        //    return RedirectToAction(nameof(Build), createGameInput);
        //}
    }
}
