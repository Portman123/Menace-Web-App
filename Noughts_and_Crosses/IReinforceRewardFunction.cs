namespace Noughts_and_Crosses
{
    public interface IReinforceRewardFunction
    {
        public void RewardDraw(Turn t, Matchbox box, Bead bead);

        public void RewardLoss(Turn t, Matchbox box, Bead bead);

        public void RewardWin(Turn t, Matchbox box, Bead bead);
    }
}
