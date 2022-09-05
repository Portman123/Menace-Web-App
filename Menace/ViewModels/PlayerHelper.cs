using Noughts_and_Crosses;

namespace Menace.ViewModels
{
    public enum PlayerType
    {
        Human,
        AIMenace,
        AIOptimal,
        AIRandom
    }

    public static class PlayerHelper
    {
        public static string GetPlayerType(Player player)
        {
            if (player is PlayerHumanOnWeb) return "Human";
            if (player is PlayerMenace) return "AIMenace";
            if (player is PlayerOptimal) return "AIOptimal";
            if (player is PlayerRandom) return "AIRandom";
            throw new Exception("Unknown player type");
        }
    }
}
