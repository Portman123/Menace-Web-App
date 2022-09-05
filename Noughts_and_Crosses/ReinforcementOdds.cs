using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class ReinforcementOdds : IReinforcement
    {
        public void Reinforce(Game g, PlayerMenace menace)
        {
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
                    if (g.Winner == null) DrawReinforcement(beadUsed, t.TurnNumber);
                    else if (g.Winner == menace) WinReinforcement(beadUsed, t.TurnNumber);
                    else if (g.Winner != menace && (g.P1 == menace || g.P2 == menace)) LossReinforcement(beadUsed, t.TurnNumber);
                    else throw new Exception("Reinforcement Error: it would seem MENACE did not play in the game given");
                }
            }
        }

        public void WinReinforcement(Bead b, int turnNumber)
        {
            try
            {
                for (int i = 0; i < turnNumber; i++)
                {
                    b.Count *= 2;
                }
            }
            catch
            {
                b.Count = int.MaxValue;
            }
        }

        public void DrawReinforcement(Bead b, int turnNumber)
        {
            for (int i = 0; i < turnNumber; i++)
            {
                b.Count *= 2;
            }
        }

        public void LossReinforcement(Bead b, int turnNumber)
        {
            for (int i = 0; i < turnNumber; i++)
            {
                b.Count = Math.Max(1, b.Count / 2);
            }
        }

        //public override void Punish(LinkedList<Bead> Beads,Bead b, int amount)
        //{
        //    for (int i = 0; i < amount; i++)
        //    {
        //        // Messy code needs cleaning up
        //        bool safe = true;
        //        foreach (Bead k in Beads)
        //        {
        //            if (k.Count >= 238609294) safe = false;
        //        }

        //        if (safe)
        //        {
        //            foreach (Bead k in Beads)
        //            {
        //                k.Double();
        //            }
        //            b.Half();
        //        }
        //    }
        //}
    }
}
