using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class GameHistory
    {
        public LinkedList<Turn> TurnHistory { get; }
        public Player P1 { get; }
        public Player P2 { get; }

        public GameHistory(Player p1, Player p2)
        {
            TurnHistory = new LinkedList<Turn>();
            P1 = p1;
            P2 = p2;
        }

        public void AddMove(Turn t)
        {
            TurnHistory.AddLast(t);
        }

        public void Display()
        {
            Console.WriteLine("");
            Console.WriteLine("Game History");
            Console.WriteLine("");
            Console.Write(P1.Name);
            Console.Write(" vs ");
            Console.Write(P2.Name);
            Console.WriteLine("");
            Console.WriteLine("");

            foreach (Turn t in TurnHistory)
            {
                Console.Write(t.MoveMaker.Name);
                Console.Write("'s Turn: ");
                Console.WriteLine("");
                Console.Write(t.X);
                Console.Write(", ");
                Console.Write(t.Y);
                Console.WriteLine("");
                t.After.PrintBoard();
            }
        }
    }
}
