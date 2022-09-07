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
            //return _context.Matchbox != null ? View(_context.Matchbox.ToList()) : Problem("Entity set 'MenaceContext.Player' is null.");
            return View();
        }

        [HttpPost]
        public IActionResult BuildSetup(GameCreate createGameInput)
        {
            if (ModelState.IsValid)
            {
                var newGame = GameFactory.CreateGame(createGameInput, _context);
                game = newGame;
                //boardPosition = new BoardPosition(newGame.CurrentBoard.Coords);

                //// (!potentially dangerous!)
                //// Assumes that one of the players are a menace player 
                //menace = newGame.P1Learner != null ? newGame.P1 as PlayerMenace: newGame.P2 as PlayerMenace;

                _context.Add(newGame);
                _context.SaveChanges();

                return RedirectToAction(nameof(Build));
            }

            return View();
        }

        public IActionResult Build()
        {
            return View(new GamePlayState());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Build([Bind("Board, CurrentPlayer")] GamePlayState game)
        {
            if (ModelState.IsValid)
            {
                //// update board position in model of game 
                //boardPosition.Encoded = game.Board;
                //game.Board = menace.PlayTurn(boardPosition).After.Encoded;

                return View(game);
            }

            return View(game);
        }

    }
}
