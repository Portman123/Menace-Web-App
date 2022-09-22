using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class PlayerOptimal : Player
    {

        public PlayerOptimal(string name) : base(name)
        {
        }

        public override Turn PlayTurn(BoardPosition CurrentBoard, int turn, int turnNumber)
        {
            int[] AIMove = AIOptimalMove.PlayTurn(CurrentBoard, turn);

            return new Turn(this, CurrentBoard, CurrentBoard.MakeMove(AIMove[0], AIMove[1], turn), AIMove[0], AIMove[1], turnNumber);
        }

        public override void LogDiagnostics()
        {
            base.LogDiagnosticsBase();
        }
    }
}
