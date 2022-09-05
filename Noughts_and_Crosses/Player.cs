using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public abstract class Player : Entity
    {
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }

        public Player(string name)
        {
            Name = name;
            Wins = 0;
            Draws = 0;
            Losses = 0;
        }

        public abstract Turn PlayTurn(BoardPosition CurrentBoard, int turn, int turnNumber);
        public abstract void LogDiagnostics();

        protected void LogDiagnosticsBase()
        {
            Console.WriteLine("");
            Console.WriteLine("-------------------------");
            Console.WriteLine("");
            Console.Write(Name);
            Console.Write(" DIAGNOSTICS");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("Wins/Draws/Losses: ");
            Console.Write(Wins);
            Console.Write(" / ");
            Console.Write(Draws);
            Console.Write(" / ");
            Console.Write(Losses);
            Console.WriteLine("");
        }

        protected void PlayTurnBase(BoardPosition CurrentBoard, int turn)
        {
            Console.WriteLine("");
            Console.Write(Name);
            Console.Write("'s Turn: ");
            Console.WriteLine("");
        }
    }
}
