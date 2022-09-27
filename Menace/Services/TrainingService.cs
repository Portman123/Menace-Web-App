using Menace.ViewModels;
using MenaceData;
using Microsoft.EntityFrameworkCore;
using Noughts_and_Crosses;

namespace Menace.Services
{
    public class TrainingService
    {
        public static void TrainOptimal(MenaceContext context, Guid menacePlayerId, int numberOfGames)
        {
            var playerOptimal = new PlayerOptimal("Optimal Trainer");

            Train(context, menacePlayerId, playerOptimal, "Optimal", numberOfGames);
        }

        public static void TrainRandom(MenaceContext context, Guid menacePlayerId, int numberOfGames)
        {
            var playerRandom = new PlayerRandom("Random Trainer");

            Train(context, menacePlayerId, playerRandom, "Random", numberOfGames);
        }

        public static void TrainMenace(MenaceContext context, Guid menacePlayerId, int numberOfGames)
        {
            // Get Menace Trainer (Pick random Menace from database)
            List<Player> MenacePlayers = context.Player.Where(p => p is PlayerMenace && p.Id != menacePlayerId).ToList();
            var MenaceTrainerId = MenacePlayers[RandomNumberGenerator.Next(MenacePlayers.Count)].Id;

            HandleMenaceTraining(context, menacePlayerId, MenaceTrainerId, "Menace", numberOfGames);
        }

        private static void Train(MenaceContext context, Guid menacePlayerId, Player trainer, string trainerName, int numberOfGames)
        {
            var playerMenace = PlayerFactory.GetPlayer(context, menacePlayerId, PlayerType.AIMenace) as PlayerMenace;

            playerMenace.StartTraining(trainerName);

            for (int i = 0; i < numberOfGames; i++)
            {
                var game1 = new Game(playerMenace, trainer);

                game1.Train();

                var game2 = new Game(trainer, playerMenace);

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



        private static void HandleMenaceTraining(MenaceContext context, Guid menacePlayerId, Guid MenacetrainerId, string trainerName, int numberOfGames)
        {
            var playerMenace = PlayerFactory.GetPlayer(context, menacePlayerId, PlayerType.AIMenace) as PlayerMenace;
            //var trainerMenace = PlayerFactory.GetPlayer(context, MenacetrainerId, PlayerType.AIMenace) as PlayerMenace;

            var trainerMenaces = context.Player
                .Where(p => p is PlayerMenace && p.Id != menacePlayerId)
                .AsNoTracking<Player>()
                .Include("MenaceEngine.Matchboxes")
                .Include("MenaceEngine.Matchboxes.Beads")
                .Include("MenaceEngine.Matchboxes.BoardPosition")
                .Include("TrainingHistory.Rounds").ToList();

            var trainerMenace = trainerMenaces[RandomNumberGenerator.Next(trainerMenaces.Count)] as PlayerMenace;

            playerMenace.StartTraining(trainerName);

            for (int i = 0; i < numberOfGames; i++)
            {
                var game1 = new Game(playerMenace, trainerMenace);

                game1.Train();

                var game2 = new Game(trainerMenace, playerMenace);

                game2.Train();
            }

            var round = playerMenace.StopTraining();


            // Add new matchboxes and beads for player
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

            //// Add new matchboxes and beads for trainer
            //context.TrainingRound.Add(round);

            foreach (Matchbox matchbox in trainerMenace.MenaceEngine.Matchboxes)
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
