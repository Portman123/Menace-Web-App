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
              return _context.Games != null ? 
                          View(await _context.Games.ToListAsync()) :
                          Problem("Entity set 'MenaceContext.Games'  is null.");
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        private Player GetPlayer(string name)
        {
            var player = _context.Player.Where(p => p.Name == name).FirstOrDefault();

            if (player == null)
            {
                player = new PlayerHumanOnWeb(name);

                _context.Player.Add(player);
            }

            return player;
        }

        public IActionResult Play()
        {
            var board = new BoardPosition();

            _context.BoardPosition.Add(board);

            var player1 = GetPlayer("Player 1");

            var player2 = GetPlayer("Player 2");

            var newGame = new GamePlayState
            {
                BoardPositionId = board.Id,
                PlayerId1 = player1.Id,
                PlayerId2 = player2.Id
            };

            _context.SaveChanges();

            return View(newGame);
        }

        private int MapPlayerLetterToPlayerNumber(string letter) => letter == "X" ? -1 : 1;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Play([Bind("Board, CurrentPlayer, BoardPositionId, PlayerId1, PlayerId2")] GamePlayState game)
        {
            if (ModelState.IsValid)
            {
                var board = _context.BoardPosition.Where(i => i.Id == game.BoardPositionId).FirstOrDefault();

                board.Encoded = game.Board;

                _context.BoardPosition.Update(board);

                var player1 = _context.Player.Where(p => p.Id == game.PlayerId1).FirstOrDefault();

                var player2 = _context.Player.Where(p => p.Id == game.PlayerId2).FirstOrDefault();

                var activePlayer = player1 is PlayerHumanOnWeb ? player2 : player1;

                var turn = activePlayer.PlayTurn(board, MapPlayerLetterToPlayerNumber(game.CurrentPlayer), board.TurnNumber);

                game.Board = turn.After.Encoded;

                _context.SaveChanges();

                return View(game);
            }

            return View(game);
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
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
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
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
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
            if (_context.Games == null)
            {
                return Problem("Entity set 'MenaceContext.Games'  is null.");
            }
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(Guid id)
        {
          return (_context.Games?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
