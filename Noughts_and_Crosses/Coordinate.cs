using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    // One day I need to sit down and make this the main system 
    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            if (x <= 2 && x >= 0 && y <= 2 && y >= 0)
            {
                X = x;
                Y = y;
            }
            else throw new Exception("Specified Coordinates out of Range for Noughts and Crosses Coord");
        }

        public bool Equals(Coordinate comparrison)
        {
            if (comparrison.X == X && comparrison.Y == Y)
            {
                return true;
            }
            return false;
        }
    }
}
