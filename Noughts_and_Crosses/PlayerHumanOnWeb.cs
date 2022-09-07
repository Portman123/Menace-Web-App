using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class PlayerHumanOnWeb : Player
    {
        public PlayerHumanOnWeb(string name) : base(name)
        {
        }

        public override void LogDiagnostics()
        {
            throw new NotImplementedException();
        }

        public override Turn PlayTurn(BoardPosition CurrentBoard, int turn, int turnNumber)
        {
            return null;
        }
    }
}
