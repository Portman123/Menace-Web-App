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
            var board = new BoardPosition();

            var player1 = GetPlayerHuman("Player 1");

            var player2 = GetPlayerMenace("Player Menace");

            var newGame = new GamePlayState
            {
                PlayerId1 = player1.Id,
                Player1Type = PlayerType.Human,
                PlayerId2 = player2.Id,
                Player2Type = PlayerType.AIMenace
            };

            _context.SaveChanges();

            return View(newGame);
        }

        private int MapPlayerLetterToPlayerNumber(string letter) => letter == "X" ? -1 : 1;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Play([Bind("Board, CurrentPlayer, PlayerId1, Player1Type, PlayerId2, Player2Type")] GamePlayState game)
        {
            if (ModelState.IsValid)
            {
                var boardBefore = new BoardPosition
                {
                    BoardPositionId = game.Board
                };

                //_context.BoardPosition.AddIfNotExists(boardBefore, b => b.BoardPositionId == boardBefore.BoardPositionId);

                var player1 = PlayerFactory.GetPlayer(_context, game.PlayerId1, game.Player1Type);

                var player2 = PlayerFactory.GetPlayer(_context, game.PlayerId2, game.Player2Type);

                // this works because it is assumed there is a human player and an AI player
                var activePlayer = player2 as PlayerMenace;

                if (activePlayer != null)
                {
                    var turn = activePlayer.PlayTurn(boardBefore, MapPlayerLetterToPlayerNumber(game.CurrentPlayer), boardBefore.TurnNumber);

                    //_context.BoardPosition.AddIfNotExists(turn.After, b => b.BoardPositionId == turn.After.BoardPositionId);

                    var matchbox = activePlayer.MenaceEngine.Matchboxes.Single(m => m.BoardPosition.BoardPositionId == boardBefore.BoardPositionId);

                    if (_context.Matchbox.AddIfNotExists(matchbox, m => m.Id == matchbox.Id))
                    {
                        //_context.Entry(matchbox.BoardPosition).State = EntityState.Unchanged;
                    }

                    foreach (var bead in matchbox.Beads)
                    {
                        _context.Bead.AddIfNotExists(bead, b => b.Id == bead.Id);
                    }

                    _context.SaveChanges();

                    // Reload UI with new game state
                    ModelState.Clear();

                    var newGameState = new GamePlayState
                    {
                        Board = turn.After.BoardPositionId,
                        PlayerId1 = player1.Id,
                        Player1Type = PlayerType.Human,
                        PlayerId2 = player2.Id,
                        Player2Type = PlayerType.AIMenace
                    };

                    return View(newGameState);
                }
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
