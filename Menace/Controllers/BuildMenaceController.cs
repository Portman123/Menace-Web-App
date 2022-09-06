using Menace.Services;
using Menace.ViewModels;
using MenaceData;
using Microsoft.AspNetCore.Mvc;
using Noughts_and_Crosses;

namespace Menace.Controllers
{
    public class BuildMenaceController : Controller
    {
        private readonly MenaceContext _context;
        //private readonly Player P1;
        //private readonly Player P2;

        public BuildMenaceController(MenaceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //return _context.Matchbox != null ? View(_context.Matchbox.ToList()) : Problem("Entity set 'MenaceContext.Player' is null.");
            return View();
        }

        [HttpPost]
        public IActionResult Index(GameCreate createGameInput)
        {
            if (ModelState.IsValid)
            {
                var newGame = GameFactory.CreateGame(createGameInput, _context);

                _context.Add(newGame);
                _context.SaveChanges();

                return RedirectToAction(nameof(Build));
            }

            return View();
        }

        public IActionResult Build()
        {
            return View();
        }

    }
}
