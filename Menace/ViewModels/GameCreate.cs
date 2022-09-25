using Noughts_and_Crosses;

namespace Menace.ViewModels
{
    public class GameCreate
    {
        public GameType GameType { get; set; }

        public ReinforcementRewardFunction.RewardFunctionType RewardFunctionType { get; set; }

        public Guid Player1Id { get; set; }

        public Guid Player2Id { get; set; }
    }
}
