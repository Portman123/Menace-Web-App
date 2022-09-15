using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class BoardPosition
    {
        public int[,] Coords { get; }

        public bool IsWinningPosition
        {
            get
            {
                return CheckWin() != 0;
            }
        }

        public bool IsGameOver
        {
            get
            {
                return IsWinningPosition || BoardFull();
            }
        }

        public int TurnNumber
        {
            get { return 9 - Encoded.Where(c => c == ' ').Count() + 1;  }
        }

        public string Encoded
        {
            get
            {
                var value = "";

                for (int i = 0; i < Coords.GetLength(0); i++)
                {
                    for (int j = 0; j < Coords.GetLength(1); j++)
                    {
                        value += GameSymbol.MapIntToSymbol(Coords[i, j]);
                    }
                }

                return value;
            }

            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    Coords[i / 3, i % 3] = GameSymbol.MapSymbolToInt(value[i]);
                }
            }
        }

        // Constructor
        public BoardPosition(int[,] coords)
        {
            Coords = coords;
        }

        // No constructor arguments passed: assume empty board
        public BoardPosition()
        {
            Coords = new int[,] { { 0,0,0},{ 0,0,0},{ 0,0,0} };
        }

        public BoardPosition(int[] coords)
        {
            // Assuming input format is left to right 
            Coords[0, 0] = coords[0];
            Coords[0, 1] = coords[1];
            Coords[0, 2] = coords[2];
            Coords[1, 0] = coords[3];
            Coords[1, 1] = coords[4];
            Coords[1, 2] = coords[5];
            Coords[2, 0] = coords[6];
            Coords[2, 1] = coords[7];
            Coords[2, 2] = coords[8];
        }

        // To make a move create a new board position and return it to whoever wants it...
        public BoardPosition MakeMove(int x, int y, int turn)
        {
            // This feels like a really stupid way to do it but it'll do for now...
            int[,] newposition = new int[,] { { Coords[0, 0], Coords[0, 1], Coords[0, 2] }, { Coords[1, 0], Coords[1, 1], Coords[1, 2] }, { Coords[2, 0], Coords[2, 1], Coords[2, 2] } };
            newposition[x, y] = turn;
            return new BoardPosition(newposition);
        }

        // Return the occupier at a given coordinate
        public int Occupant (Coordinate location)
        {
            return Coords[location.X, location.Y];
        }

        // There are 8 winning positions that need to be checked
        public int CheckWin()
        {
            // Top Row
            if (Coords[0, 0] == Coords[0, 1] && Coords[0, 1] == Coords[0, 2]) return Coords[0, 0];
            // Middle Row
            if (Coords[1, 0] == Coords[1, 1] && Coords[1, 1] == Coords[1, 2]) return Coords[1, 0];
            // Bottom Row
            if (Coords[2, 0] == Coords[2, 1] && Coords[2, 1] == Coords[2, 2]) return Coords[2, 0];
            // Left Column
            if (Coords[0, 0] == Coords[1, 0] && Coords[1, 0] == Coords[2, 0]) return Coords[0, 0];
            // Middle Column
            if (Coords[0, 1] == Coords[1, 1] && Coords[1, 1] == Coords[2, 1]) return Coords[0, 1];
            // Right Column
            if (Coords[0, 2] == Coords[1, 2] && Coords[1, 2] == Coords[2, 2]) return Coords[0, 2];
            // First Diagonal 
            if (Coords[0, 0] == Coords[1, 1] && Coords[1, 1] == Coords[2, 2]) return Coords[0, 0];
            // Second Diagonal
            if (Coords[0, 2] == Coords[1, 1] && Coords[1, 1] == Coords[2, 0]) return Coords[0, 2];

            // nobody has won yet
            return 0;
        }

        public bool BoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Coords[i, j] == 0) return false;
                }
            }
            return true;
        }

        public bool SameAs(BoardPosition comparrison)
        {
            for (int x=0; x < 3; x++)
            {
                for (int y=0; y < 3; y++)
                {
                    if (!(Coords[x, y] == comparrison.Coords[x,y])) return false;
                }
            }
            return true;
        }

        public void PrintBoard()
        {
            // Build string to represent the game BoardPosition using ascci
            StringBuilder output = new StringBuilder();

            // (To make it look nicer)
            output.Append("-------------");
            output.Append("\n");

            for (int i = 0; i < 3; i++)
            {
                output.Append("|");
                for (int j = 0; j < 3; j++)
                {
                    // Interpret BoardPosition data
                    if (Coords[i, j] == -1) { output.Append(" X "); }
                    else if (Coords[i, j] == 1) { output.Append(" O "); }
                    else if (Coords[i, j] == 0) { output.Append(" - "); }
                    else { output.Append("ERROR: Positional data is not in the correct format."); }
                    output.Append("|");
                }
                output.Append("\n");
                output.Append("-------------");
                output.Append("\n");
            }

            // Print result to console
            Console.WriteLine(output.ToString());
        }

        // static version so I can print a board without an instance. 
        public static void PrintBoard(int[,] boardPosition)
        {
            // Build string to represent the game BoardPosition using ascci
            StringBuilder output = new StringBuilder();

            // (To make it look nicer)
            output.Append("-------------");
            output.Append("\n");

            for (int i = 0; i < 3; i++)
            {
                output.Append("|");
                for (int j = 0; j < 3; j++)
                {
                    // Interpret BoardPosition data
                    if (boardPosition[i, j] == -1) { output.Append(" X "); }
                    else if (boardPosition[i, j] == 1) { output.Append(" O "); }
                    else if (boardPosition[i, j] == 0) { output.Append(" - "); }
                    else { output.Append("ERROR: Positional data is not in the correct format."); }
                    output.Append("|");
                }
                output.Append("\n");
                output.Append("-------------");
                output.Append("\n");
            }

            // Print result to console
            Console.WriteLine(output.ToString());
        }
    }
}
