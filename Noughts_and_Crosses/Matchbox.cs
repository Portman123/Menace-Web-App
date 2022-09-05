using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class Matchbox : Entity
    {
        // Matchbox ID
        public BoardPosition BoardPosition { get; set; }

        public LinkedList<Bead> Beads { get; set; }

        // Constructors
        public Matchbox() { }

        public Matchbox(BoardPosition boardPosition, LinkedList<Bead> beads)
        {
            BoardPosition = boardPosition;
            Beads = beads;
        }

        public Matchbox(BoardPosition boardPosition)
        {
            BoardPosition = boardPosition;
            Beads = new LinkedList<Bead>();
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (BoardPosition.Coords[i, j] == 0)
                    {
                        Beads.AddLast(new Bead(i, j));
                    }
                }
            }
        }

        public Bead GetBead(int x, int y)
        {
            foreach (Bead b in Beads)
            {
                if (b.X == x && b.Y == y) return b;
            }
            return null;
        }

        public int GetNumberOfBeads()
        {
            int count = 0;
            foreach (Bead b in Beads)
            {
                count += b.Count;
            }
            return count;
        }

        //This is where I'm starting to see I need a coordinate object
        public int[] Shake()
        {
            // Generate random number
            int randomNumber = RandomNumberGenerator.Next(GetNumberOfBeads());

            // Determine which move the random number points to
            int count = 0;

            foreach (Bead move in Beads)
            {
                count += move.Count;

                if (randomNumber < count) return new int[] {move.X, move.Y};
            }

            throw new Exception("Something went wrong while MENACE was picking it's next move.");
        }

        //public void ScoreWin(int x, int y, int turnNumber)
        //{
        //    Reinforcer.WinReinforcement(GetBead(x, y), turnNumber);
        //}

        //public void ScoreDraw(int x, int y, int turnNumber)
        //{
        //    Reinforcer.DrawReinforcement(GetBead(x, y), turnNumber);
        //}

        //public void ScoreLoss(int x, int y, int turnNumber)
        //{
        //    Reinforcer.LossReinforcement(GetBead(x, y), turnNumber);
        //}
    }
}
