namespace Noughts_and_Crosses
{
    public class ReinforceThreePerWinOnePerDraw : IReinforceRewardFunction
    {
        public void RewardDraw(Turn t, Matchbox box, Bead bead)
        {
            bead.Count += 1;
        }

        public void RewardLoss(Turn t, Matchbox box, Bead bead)
        {
        }

        public void RewardWin(Turn t, Matchbox box, Bead bead)
        {
            bead.Count += 3;
        }
    }
}
