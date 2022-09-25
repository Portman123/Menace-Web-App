namespace Noughts_and_Crosses
{
    public class ReinforceWinPlusTurnLossMinusTurn : IReinforceRewardFunction
    {
        public void RewardDraw(Turn t, Matchbox box, Bead bead)
        {
        }

        public void RewardLoss(Turn t, Matchbox box, Bead bead)
        {
            bead.Count = bead.Count - t.TurnNumber;

            // nothing could go wrong...
            if (bead.Count < 1)
            {
                var deficit = -bead.Count;

                foreach (Bead b in box.Beads)
                {
                    b.Count += deficit + 1;
                }
            }
        }

        public void RewardWin(Turn t, Matchbox box, Bead bead)
        {
            bead.Count += t.TurnNumber;
        }
    }
}
