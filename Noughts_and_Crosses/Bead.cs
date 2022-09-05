using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class Bead : Entity
    {
        public int X { get; set;  }
        public int Y { get; set;  }
        public int Count { get; set; }

        public Bead(int x, int y)
        {
            Count = 1;

            if (x <= 2 && x >= 0 && y <= 2 && y >= 0)
            {
                X = x;
                Y = y;
            }
            else throw new Exception("Specified Coordinates out of Range for Noughts and Crosses Coord");
        }

        public void Double()
        {
            if (Count < 238609294) Count *= 2;
        }
        public void Half()
        {
            Count /= 2;
        }

        public void Increment()
        {
            Count++;
        }
        public void Decrement()
        {
            Count--;
        }
    }
}
