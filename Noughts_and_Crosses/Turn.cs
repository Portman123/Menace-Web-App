using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class Turn
    {
        public Player MoveMaker { get; }
        public BoardPosition Before { get; }
        public BoardPosition After { get; }
        public int X { get; }
        public int Y { get; }
        public int TurnNumber { get; }

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
