using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class Turn : Entity
    {
        public Player MoveMaker { get; set; }
        public BoardPosition Before { get; set; }
        public BoardPosition After { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int TurnNumber { get; set; }

        public Turn()
        {
        }

        public Turn (Player moveMaker, BoardPosition before, BoardPosition after, int x, int y, int turnNumber)
        {
            MoveMaker = moveMaker;
            Before = before;
            After = after;
            X = x;
            Y = y;
            TurnNumber = turnNumber;
        }

        // In future
            // Make one that figures out what the X and Y was...
    }
}
