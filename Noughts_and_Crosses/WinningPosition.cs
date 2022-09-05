using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class WinningPosition
    {
        public Coordinate[] Coordinates { get; set; }

        public WinningPosition(Coordinate[] coords)
        {
            Coordinates = coords;
        }

        public bool Equals(WinningPosition comparison)
        {
            for (int i = 0; i < Coordinates.Length; i++)
            {
                if (Coordinates[i] != comparison.Coordinates[i]) return false;
            }
            return true;
        }

        public bool Intersects(WinningPosition comparison)
        {
            // ignore intersections between the same position
            if (Equals(comparison)) return false;

            // check if any coordinate in this position is a coordinate in the comparison
            for (int i = 0; i < Coordinates.Length; i++)
            {
                for (int j=0; j < comparison.Coordinates.Length; j++)
                {
                    if (comparison.Coordinates[j].Equals(Coordinates[i])) return true;
                }
            }
            return false;
        }

        public Coordinate IntersectCoord(WinningPosition comparison)
        {
            // ignore intersections between the same position
            if (Equals(comparison)) return null;

            // check if any coordinate in this position is a coordinate in the comparison
            for (int i = 0; i < Coordinates.Length; i++)
            {
                for (int j = 0; j < comparison.Coordinates.Length; j++)
                {
                    if (comparison.Coordinates[j].Equals(Coordinates[i])) return Coordinates[i];
                }
            }
            return null;
        }

    }
}
