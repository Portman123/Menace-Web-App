using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class ReinforcementIncremental : IReinforcement
    {
        public void Reinforce(Game g, PlayerMenace menace)
        {
            // Ensure menace played in the game given
            if (g.P1 != menace && g.P2 != menace) throw new Exception("Reinforcement Error: it would seem MENACE did not play in the game given");

            // for each turn in the game's history where menace played a turn
            foreach (Turn t in g.History.TurnHistory)
            {
                if (t.MoveMaker == menace)
                {
                    // Fetch the box Menace used to play the turn
                    Matchbox boxUsed = menace.MenaceEngine.MatchboxByBoardPos(t.Before);

                    // Fetch the bead that Menace selected from the boxUsed by checking coordinates played that turn
                    Bead beadUsed = boxUsed.GetBead(t.X, t.Y);

                    // Determine reinforcement type based on the game outcome

                    if (g.Winner == null)
                    {
                        DrawReinforcement(beadUsed, t.TurnNumber);
                    }
                    else if (g.Winner == menace)
                    {
                        WinReinforcement(beadUsed, t.TurnNumber);
                    }
                    else if (g.Winner != menace)
                    {
                        LossReinforcement(beadUsed, t.TurnNumber);
                    }
                }
            }
        }

        public void WinReinforcement(Bead b, int turnNumber)
        {
            b.Count += turnNumber*3;
        }

        public void DrawReinforcement(Bead b, int turnNumber)
        {
            b.Count += turnNumber;
        }

        public void LossReinforcement(Bead b, int turnNumber)
        {
            b.Count = Math.Max(1, b.Count - turnNumber);
        }
    }
}
