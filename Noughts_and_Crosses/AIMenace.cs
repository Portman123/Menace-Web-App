using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Noughts_and_Crosses
{
    public class AIMenace : Entity, IAIEngine
    {
        // List of all Matchboxes
        public LinkedList<Matchbox> Matchboxes { get; set; }

        //[NotMapped]
        //public IReinforcement Reinforcer { get; set; }

        // Constructor
        public AIMenace()
        {
            Matchboxes = new LinkedList<Matchbox>();
            //Reinforcer = new ReinforcementIncremental();
        }

        public void AddMatchbox(BoardPosition newBoardPos)
        {
            Matchboxes.AddLast(new Matchbox(newBoardPos));
        }

        // Find Matchbox/Check Existence in Menace by specifying board position
        public Matchbox MatchboxByBoardPos(BoardPosition boardPosition) //=> Matchboxes.Where(m => m.BoardPosition.SameAs(boardPosition)).FirstOrDefault();
        {
            // If the matchbox exists in MENACE then return it
            foreach (Matchbox box in Matchboxes)
            {
                if (box.BoardPosition.SameAs(boardPosition))
                {
                    return box;
                }
            }
            // Return null if it doesn't
            return null;
        }

        public bool CheckExistence(BoardPosition boardPosition) => Matchboxes.Any(m => m.BoardPosition.SameAs(boardPosition));

        // DIAGNOSTIC METHODS
        //  Returns number of Matchboxes present in MENACE
        public int NumOfMatchboxes()
        {
            return Matchboxes.Count();
        }
        // returns the number of edges present in the graph
        public int NumOfMoves()
        {
            int count = 0;
            foreach (Matchbox box in Matchboxes)
            {
                count += box.Beads.Count;
            }
            return count;
        }

        public int NumOfBeads()
        {
            int count = 0;
            foreach (Matchbox box in Matchboxes)
            {
                count += box.GetNumberOfBeads();
            }
            return count;
        }

        //This is where I'm starting to see I need a coordinate object
        public int[] PlayTurn(BoardPosition boardPos, int turn)
        {
            // If Menace hasn't encountered this position before
            if (!CheckExistence(boardPos))
            {
                AddMatchbox(boardPos);
                Console.WriteLine("Adding new position to MENACE");
                boardPos.PrintBoard();
            }

            Matchbox box = MatchboxByBoardPos(boardPos);

            return box.Shake();
        }

        public void Reinforce(Game g, PlayerMenace p)
        {
            ReinforcementIncremental.Reinforce(g.History, p);
        }

        //public void Reward(GameHistory game, string nameCheck)
        //{
        //    foreach (Turn t in game.TurnHistory)
        //    {
        //        if (t.MoveMaker.Name == nameCheck)
        //        {
        //            // This should be broken down into methods that are part of the corresponding classes
        //            MatchboxByBoardPos(t.Before).ScoreWin(t.X,t.Y,t.TurnNumber); // This needs to be some function
        //        }
        //    }
        //}

        //public void DrawReward(GameHistory game, string nameCheck)
        //{
        //    foreach (Turn t in game.TurnHistory)
        //    {
        //        if (t.MoveMaker.Name == nameCheck)
        //        {
        //            MatchboxByBoardPos(t.Before).ScoreDraw(t.X, t.Y,t.TurnNumber);
        //        }
        //    }
        //}

        //public void Punish(GameHistory game, string nameCheck)
        //{
        //    foreach (Turn t in game.TurnHistory)
        //    {
        //        // This should be broken down into methods that are part of the corresponding classes
        //        if (t.MoveMaker.Name == nameCheck)
        //        {
        //            MatchboxByBoardPos(t.Before).ScoreLoss(t.X, t.Y,t.TurnNumber);
        //        }
        //    }
        //}
    }
}
