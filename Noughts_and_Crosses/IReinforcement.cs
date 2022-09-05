using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public interface IReinforcement
    {
        void Reinforce(Game g, PlayerMenace menace);

        void WinReinforcement(Bead b, int turnNumber);

        void DrawReinforcement(Bead b, int turnNumber);

        void LossReinforcement(Bead b, int turnNumber);
    }
}
