using Menace.ViewModels;
using MenaceData;
using Noughts_and_Crosses;

namespace Menace.Services
{
    public class TrainingService
    {
        public static void TrainRandom(MenaceContext context, Guid menacePlayerId)
        {
            var playerRandom = new PlayerRandom("Random Trainer");

            var playerMenace = PlayerFactory.GetPlayer(context, menacePlayerId, PlayerType.AIMenace) as PlayerMenace;

            playerMenace.StartTraining("Random");

            for (int i = 0; i < 10; i++)
            {
                var game1 = new Game(playerMenace, playerRandom);

                game1.Train();

                var game2 = new Game(playerRandom, playerMenace);

                game2.Train();
            }

            var round = playerMenace.StopTraining();

            // Add new matchboxes and beads.
            context.TrainingRound.Add(round);

            foreach (Matchbox matchbox in playerMenace.MenaceEngine.Matchboxes)
            {
                matchbox.BoardPosition = context.BoardPosition.GetOrAddIfNotExists(matchbox.BoardPosition, b => b.BoardPositionId == matchbox.BoardPosition.BoardPositionId);

                if (context.Matchbox.AddIfNotExists(matchbox, m => m.Id == matchbox.Id))
                {
                    context.Entry(matchbox).Reference(m => m.BoardPosition).IsModified = false;

                    foreach (var bead in matchbox.Beads)
                    {
                        context.Bead.Add(bead);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
