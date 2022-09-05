using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class WinningPositionIntersect
    {
        public WinningPosition WinPos1 { get; set; }
        public WinningPosition WinPos2 { get; set; }
        public Coordinate IntersectCoord { get; set; }

        public WinningPositionIntersect(WinningPosition winPos1, WinningPosition winPos2)
        {
            WinPos1 = winPos1;
            WinPos2 = winPos2;
            IntersectCoord = winPos1.IntersectCoord(winPos2);
        }

        public bool Equals(WinningPositionIntersect comparison)
        {
            if (WinPos1 == comparison.WinPos1 && WinPos2 == comparison.WinPos2) return true;
            if (WinPos1 == comparison.WinPos2 && WinPos2 == comparison.WinPos1) return true;
            return false;
        }
    }
}
