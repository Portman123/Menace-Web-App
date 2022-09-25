using Menace.ViewModels;
using Noughts_and_Crosses;
using MenaceData;

namespace Menace.Services
{
    public class GameFactory
    {
        public static Game CreateGame(GameCreate input, MenaceContext context)
        {
            PlayerMenace playerMenace = new PlayerMenace(new AIMenace(),$"Player {context.Player.Count()}", ReinforcementRewardFunction.RewardFunctionType.ThreePerWinOnePerDraw);
            Player playerHuman = new PlayerHumanOnWeb($"Player {context.Player.Count() + 1}");

            switch (input.GameType)
            {
                case GameType.MenaceP1:
                    return new Game(playerMenace, playerHuman);
                case GameType.MenaceP2:
                    return new Game(playerHuman, playerMenace);
                default:
                    throw new Exception($"Unexpected game type{input.GameType}");
            }
        }
    }
}
