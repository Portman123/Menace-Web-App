using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class ReinforcementIncremental
    {
        public static void Reinforce(GameHistory g, PlayerMenace menace)
        {
            // Ensure menace played in the game given
            if (g.P1 != menace && g.P2 != menace) throw new Exception("Reinforcement Error: it would seem MENACE did not play in the game given");

            var rewardFunction = ReinforcementRewardFunction.GetRewardFunction(menace.ReinforcementType);

            // for each turn in the game's history where menace played a turn
            foreach (Turn t in g.Turns)
            {
                if (t.TurnPlayer == menace)
                {
                    // Fetch the box Menace used to play the turn
                    Matchbox boxUsed = menace.MenaceEngine.MatchboxByBoardPos(t.Before);

                    // Fetch the bead that Menace selected from the boxUsed by checking coordinates played that turn
                    Bead beadUsed = boxUsed.GetBead(t.X, t.Y);

                    // Determine reinforcement type based on the game outcome

                    if (g.Winner == null)
                    {
                        boxUsed.Draws++;

                        rewardFunction.RewardDraw(t, boxUsed, beadUsed);

                    }
                    else if (g.Winner == menace)
                    {
                        boxUsed.Wins++;

                        rewardFunction.RewardWin(t, boxUsed, beadUsed);
                    }
                    else if (g.Winner != menace)
                    {
                        boxUsed.Losses++;

                        rewardFunction.RewardLoss(t, boxUsed, beadUsed);
                    }
                }
            }
        }
    }
}
