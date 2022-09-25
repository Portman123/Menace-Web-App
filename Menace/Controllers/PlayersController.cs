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
    public class PlayersController : Controller
    {
        private readonly MenaceContext _context;

        public PlayersController(MenaceContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            return _context.Player != null ? View(await _context.Player.OrderBy(p => -p.Wins).ThenBy(p => -p.Draws).ThenBy(p => p.Losses).ThenByDescending(p => p.Name).ToListAsync()) : Problem("Entity set 'MenaceContext.Player' is null.");
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Player == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            PlayerMenace menacePlayer = PlayerFactory.GetPlayer(_context, player.Id, PlayerType.AIMenace) as PlayerMenace;

            var details = new MenaceDetails
            {
                Player = player,
                PlayerId = player.Id,
                menaceEngine = menacePlayer.MenaceEngine,
                Matchboxes = menacePlayer.MenaceEngine.Matchboxes,
                TrainingHistory = menacePlayer.TrainingHistory
            };

            return View(details);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type")] PlayerCreate createPlayerInput)
        {
            if (ModelState.IsValid)
            {
                var player = PlayerFactory.CreatePlayer(createPlayerInput, _context.Player.Count() + 1);

                _context.Add(player);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(createPlayerInput);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Player == null)
            {
                return NotFound();
            }

            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Wins,Draws,Losses,Id")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Player == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Player == null)
            {
                return Problem("Entity set 'MenaceContext.Player'  is null.");
            }
            var player = await _context.Player.FindAsync(id);
            if (player != null)
            {
                // First remove all entities with foreign keys pointing to player
                foreach (Game g in _context.Game)
                {
                    if (g.P1 != null && g.P1.Id == player.Id) _context.Game.Remove(g);
                    else if (g.P2 != null && g.P2.Id == player.Id) _context.Game.Remove(g);
                }
                await _context.SaveChangesAsync();
                _context.Player.Remove(player);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Train([Bind("PlayerId, TrainingOption")] MenaceDetails menaceDetails)
        {
            if (menaceDetails.TrainingOption == "random")
            {
                TrainingService.TrainRandom(_context, menaceDetails.PlayerId);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(Guid id)
        {
          return (_context.Player?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
