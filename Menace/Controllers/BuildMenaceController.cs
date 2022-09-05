using MenaceData;
using Microsoft.AspNetCore.Mvc;
using Noughts_and_Crosses;

namespace Menace.Controllers
{
    public class BuildMenaceController : Controller
    {
        private readonly MenaceContext _context;
        private readonly Player P1;
        private readonly Player P2;

        public BuildMenaceController(MenaceContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Build()
        {
            return View();
        }

    }
}
