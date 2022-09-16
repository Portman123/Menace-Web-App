using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MenaceData;
using Noughts_and_Crosses;
using Menace.ViewModels;
using Menace.Services;

namespace Menace.Controllers
{
    public class GamesController : Controller
    {
        private readonly MenaceContext _context;

        public GamesController(MenaceContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            return _context.Game != null ?
                        View(await _context.Game.ToListAsync()) :
                        Problem("Entity set 'MenaceContext.Games'  is null.");
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
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

        public IActionResult Play()
        {
            var player1 = GetPlayerHuman("Player 1");

            var player2 = GetPlayerMenace("Player Menace");

            var newGame = new GameHistory(player1, player2);

            _context.Add(newGame);

            _context.SaveChanges();

            var gameState = new GamePlayState
            {
                BoardBeforeInput = GamePlayState.WrapBoard(BoardPosition.EmptyBoardPostion),
                GameHistoryId = newGame.Id
            };

            return View(gameState);
        }

        private int MapPlayerLetterToPlayerNumber(string letter) => letter == "X" ? -1 : 1;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Play([Bind("BoardBeforeInput, BoardAfterInput, CurrentPlayerSymbol, GameHistoryId")] GamePlayState gameState)
        {
            if (ModelState.IsValid)
            {
                // Load state from inputs
                var boardBeforeInput = new BoardPosition
                {
                    BoardPositionId = GamePlayState.UnwrapBoard(gameState.BoardBeforeInput)
                };

                var boardAfterInput = new BoardPosition
                {
                    BoardPositionId = GamePlayState.UnwrapBoard(gameState.BoardAfterInput)
                };

                var game = _context.GameHistory.Where(g => g.Id == gameState.GameHistoryId)
                    .Include(g => g.P1)
                    .Include(g => g.P2)
                    .Include(g => g.Turns)
                    .Single();

                var humanPlayer = PlayerFactory.GetPlayer(_context, game.P1.Id, PlayerType.Human);

                var aiPlayer = PlayerFactory.GetPlayer(_context, game.P2.Id, PlayerType.AIMenace) as PlayerMenace;

                // Add turn just played to Game History
                var humanMove = BoardPosition.GetMove(boardBeforeInput, boardAfterInput);

                var humanTurn = new Turn(humanPlayer, boardBeforeInput, boardAfterInput, humanMove.X, humanMove.Y, boardAfterInput.TurnNumber);

                //game.AddMove(humanTurn);

                // Make AI play its turn
                var aiTurn = aiPlayer.PlayTurn(boardAfterInput, MapPlayerLetterToPlayerNumber(gameState.CurrentPlayerSymbol), boardAfterInput.TurnNumber);

                //game.AddMove(aiTurn);

                //var matchbox = aiPlayer.MenaceEngine.Matchboxes.Single(m => m.BoardPosition.BoardPositionId == boardAfterInput.BoardPositionId);

                //_context.Matchbox.AddIfNotExists(matchbox, m => m.Id == matchbox.Id);

                _context.SaveChanges();

                // Reload UI with new game state
                ModelState.Clear();

                var newGameState = new GamePlayState
                {
                    BoardBeforeInput = GamePlayState.WrapBoard(aiTurn.After.BoardPositionId),
                    GameHistoryId = gameState.GameHistoryId
                };

                return View(newGameState);
            }

            return View(gameState);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Finished,TurnNumber,Id")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.Id = Guid.NewGuid();
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Finished,TurnNumber,Id")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'MenaceContext.Games'  is null.");
            }
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(Guid id)
        {
            return (_context.Game?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
