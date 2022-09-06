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
    public class MatchboxesController : Controller
    {
        private readonly MenaceContext _context;

        public MatchboxesController(MenaceContext context)
        {
            _context = context;
        }

        // GET: Matchboxes
        public async Task<IActionResult> Index()
        {
              return _context.Matchbox != null ? 
                          View(await _context.Matchbox.ToListAsync()) :
                          Problem("Entity set 'MenaceContext.Matchbox'  is null.");
        }

        // GET: Matchboxes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Matchbox == null)
            {
                return NotFound();
            }

            var matchbox = await _context.Matchbox
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matchbox == null)
            {
                return NotFound();
            }

            return View(matchbox);
        }

        // GET: Matchboxes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Matchboxes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Matchbox matchbox)
        {
            if (ModelState.IsValid)
            {
                matchbox.Id = Guid.NewGuid();
                _context.Add(matchbox);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(matchbox);
        }

        // GET: Matchboxes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Matchbox == null)
            {
                return NotFound();
            }

            var matchbox = await _context.Matchbox.FindAsync(id);
            if (matchbox == null)
            {
                return NotFound();
            }
            return View(matchbox);
        }

        // POST: Matchboxes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id")] Matchbox matchbox)
        {
            if (id != matchbox.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matchbox);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchboxExists(matchbox.Id))
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
            return View(matchbox);
        }

        // GET: Matchboxes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Matchbox == null)
            {
                return NotFound();
            }

            var matchbox = await _context.Matchbox
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matchbox == null)
            {
                return NotFound();
            }

            return View(matchbox);
        }

        // POST: Matchboxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Matchbox == null)
            {
                return Problem("Entity set 'MenaceContext.Matchbox'  is null.");
            }
            var matchbox = await _context.Matchbox.FindAsync(id);
            if (matchbox != null)
            {
                _context.Matchbox.Remove(matchbox);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchboxExists(Guid id)
        {
          return (_context.Matchbox?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
