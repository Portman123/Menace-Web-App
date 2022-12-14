using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class ReinforcementIncremental
    {
/*
        public void Reinforce(Game g, PlayerMenace menace)
        {
            // Ensure menace played in the game given
            if (g.P1 != menace && g.P2 != menace) throw new Exception("Reinforcement Error: it would seem MENACE did not play in the game given");

            // for each turn in the game's history where menace played a turn
            foreach (Turn t in g.History.Turns)
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

                        DrawReinforcement(beadUsed, t.TurnNumber);
                    }
                    else if (g.Winner == menace)
                    {
                        boxUsed.Wins++;

                        WinReinforcement(beadUsed, t.TurnNumber);
                    }
                    else if (g.Winner != menace)
                    {
                        boxUsed.Losses++;

                        LossReinforcement(beadUsed, t.TurnNumber);
                    }
                }
            }
        }
*/
        public static void Reinforce(GameHistory g, PlayerMenace menace)
        {
            // Ensure menace played in the game given
            if (g.P1 != menace && g.P2 != menace) throw new Exception("Reinforcement Error: it would seem MENACE did not play in the game given");

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
                    }
                    else if (g.Winner == menace)
                    {
                        boxUsed.Wins++;
                        beadUsed.Count += t.TurnNumber;
                    }
                    else if (g.Winner != menace)
                    {
                        //boxUsed.Losses++;
                        //beadUsed.Count = Math.Max(1, beadUsed.Count - t.TurnNumber);

                        boxUsed.Losses++;

                        beadUsed.Count = beadUsed.Count - t.TurnNumber;

                        // nothing could go wrong...
                        if (beadUsed.Count < 1)
                        {
                            var deficit = -beadUsed.Count;

                            foreach (Bead b in boxUsed.Beads)
                            {
                                b.Count += deficit + 1;
                            }
                        }
                    }
                }
            }
        }
    }
}
