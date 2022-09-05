using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MenaceData;
using Noughts_and_Crosses;

namespace Menace.Controllers
{
    public class GameHistoriesController : Controller
    {
        private readonly MenaceContext _context;

        public GameHistoriesController(MenaceContext context)
        {
            _context = context;
        }

        // GET: GameHistories
        public async Task<IActionResult> Index()
        {
              return _context.GameHistories != null ? 
                          View(await _context.GameHistories.ToListAsync()) :
                          Problem("Entity set 'MenaceContext.GameHistories'  is null.");
        }

        // GET: GameHistories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.GameHistories == null)
            {
                return NotFound();
            }

            var gameHistory = await _context.GameHistories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameHistory == null)
            {
                return NotFound();
            }

            return View(gameHistory);
        }

        // GET: GameHistories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] GameHistory gameHistory)
        {
            if (ModelState.IsValid)
            {
                gameHistory.Id = Guid.NewGuid();
                _context.Add(gameHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameHistory);
        }

        // GET: GameHistories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.GameHistories == null)
            {
                return NotFound();
            }

            var gameHistory = await _context.GameHistories.FindAsync(id);
            if (gameHistory == null)
            {
                return NotFound();
            }
            return View(gameHistory);
        }

        // POST: GameHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id")] GameHistory gameHistory)
        {
            if (id != gameHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameHistoryExists(gameHistory.Id))
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
            return View(gameHistory);
        }

        // GET: GameHistories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.GameHistories == null)
            {
                return NotFound();
            }

            var gameHistory = await _context.GameHistories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameHistory == null)
            {
                return NotFound();
            }

            return View(gameHistory);
        }

        // POST: GameHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.GameHistories == null)
            {
                return Problem("Entity set 'MenaceContext.GameHistories'  is null.");
            }
            var gameHistory = await _context.GameHistories.FindAsync(id);
            if (gameHistory != null)
            {
                _context.GameHistories.Remove(gameHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameHistoryExists(Guid id)
        {
          return (_context.GameHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
