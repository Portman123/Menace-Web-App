using System;
using System.Collections.Generic;

namespace Noughts_and_Crosses
{
    public class PlayerMenace : Player, IReinforcementLearner
    {
        public AIMenace MenaceEngine { get; set; }

        public PlayerMenace(string name) : base(name)
        {
        }

        public PlayerMenace(AIMenace menaceEngine, string name) : base(name)
        {
            MenaceEngine = menaceEngine;
        }

        public override Turn PlayTurn(BoardPosition CurrentBoard, int turn, int turnNumber)
        {
            int[] AIMove = MenaceEngine.PlayTurn(CurrentBoard, turn);

            return new Turn(this, CurrentBoard, CurrentBoard.MakeMove(AIMove[0], AIMove[1], turn), AIMove[0], AIMove[1], turnNumber);
        }

        public Turn PlayTurn(BoardPosition CurrentBoard)
        {
            int turn = new int();
            int turnNumber = 1;

            for(int i=0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (CurrentBoard.Coords[i, j] != 0) turnNumber++;
                }
            }
            turn = turnNumber % 2 == 0 ? -1 : 1;

            return PlayTurn(CurrentBoard, turn, turnNumber);
        }

        public void Reinforce(Game g)
        {
            MenaceEngine.Reinforce(g, this);
        }

        public override void LogDiagnostics()
        {
            base.LogDiagnosticsBase();

            Console.Write("Number of Matchboxes: ");
            Console.Write(MenaceEngine.NumOfMatchboxes());
            Console.WriteLine("");
            Console.Write("Number of Moves Known: ");
            Console.Write(MenaceEngine.NumOfMoves());
            Console.WriteLine("");
            Console.Write("Number of Beads: ");
            Console.Write(MenaceEngine.NumOfBeads());
        }
    }
}
