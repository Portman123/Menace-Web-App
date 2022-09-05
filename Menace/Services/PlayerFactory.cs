using Menace.ViewModels;
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
    }
}
