using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class PlayerOptimal : Player
    {
        public AIOptimalMove OptimalEngine { get; set; }

        public PlayerOptimal(AIOptimalMove optimalEngine, string name) : base(name)
        {
            OptimalEngine = optimalEngine;
        }

        public override Turn PlayTurn(BoardPosition CurrentBoard, int turn, int turnNumber)
        {
            int[] AIMove = OptimalEngine.PlayTurn(CurrentBoard, turn);

            return new Turn(this, CurrentBoard, CurrentBoard.MakeMove(AIMove[0], AIMove[1], turn), AIMove[0], AIMove[1], turnNumber);
        }

        public override void LogDiagnostics()
        {
            base.LogDiagnosticsBase();
        }
    }
}
