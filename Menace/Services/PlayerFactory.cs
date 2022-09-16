using Menace.ViewModels;
using MenaceData;
using Microsoft.EntityFrameworkCore;
using Noughts_and_Crosses;

namespace Menace.Services
{
    public static class PlayerFactory
    {
        public static Player CreatePlayer(PlayerCreate input, int playerCount)
        {
            var playerName = $"Player {playerCount}";

            switch (input.Type)
            {
                case PlayerType.Human:
                    return new PlayerHumanOnWeb(playerName);
                case PlayerType.AIOptimal:
                    return new PlayerOptimal(new AIOptimalMove(), playerName);
                case PlayerType.AIRandom:
                    return new PlayerRandom(new AIRandomMove(), playerName);
                case PlayerType.AIMenace:
                    return new PlayerMenace(new AIMenace(), playerName);
                default:
                    throw new Exception($"Unexpected player type {input.Type}");
            }
        }

        public static Player GetPlayer(MenaceContext context, Guid playerId, PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Human:
                    return context.Player.Where(p => p.Id == playerId).Single();
                case PlayerType.AIOptimal:
                    throw new NotImplementedException();
                case PlayerType.AIRandom:
                    throw new NotImplementedException();
                case PlayerType.AIMenace:
                    var menace = context.PlayerMenace
                        .Where(p => p.Id == playerId)
                        .Include("MenaceEngine.Matchboxes")
                        .Include("MenaceEngine.Matchboxes.Beads")
                        .Include("MenaceEngine.Matchboxes.BoardPosition")
                        .Single();
                    return menace;
                default:
                    throw new Exception($"Unexpected player type {playerType}");
            }

        }
    }
}
