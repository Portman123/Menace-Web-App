namespace Noughts_and_Crosses
{
    public static class ReinforcementRewardFunction
    {
        public enum RewardFunctionType
        {
            ThreePerWinOnePerDraw,
            WinPlusTurnLossMinusTurn
        }

        public static IReinforceRewardFunction GetRewardFunction(RewardFunctionType t)
        {
            switch (t)
            {
                case RewardFunctionType.ThreePerWinOnePerDraw:
                    return new ReinforceThreePerWinOnePerDraw();
                case RewardFunctionType.WinPlusTurnLossMinusTurn:
                    return new ReinforceWinPlusTurnLossMinusTurn();
                default: throw new NotImplementedException($"Unrecognized reinforment reward function type {t}");
            }
        }

        public static string MapRewardFunctionTypeToName(RewardFunctionType t)
        {
            switch (t)
            {
                case RewardFunctionType.ThreePerWinOnePerDraw:
                    return "Add three beads per win; one bead per draw";
                case RewardFunctionType.WinPlusTurnLossMinusTurn:
                    return "Add/subtract turn number x beads per win/loss";
                default: throw new NotImplementedException($"Unrecognized reinforment reward function type {t}");
            }
        }
    }
}
