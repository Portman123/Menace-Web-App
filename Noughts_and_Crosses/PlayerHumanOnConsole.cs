using System;

namespace Noughts_and_Crosses
{
    public class PlayerHumanOnConsole : Player
    {
        public PlayerHumanOnConsole(string name) : base(name)
        {
        }

        public override void LogDiagnostics()
        {
            // Move along please... nothing to see here...
        }

        public override Turn PlayTurn(BoardPosition CurrentBoard, int turn, int turnNumber)
        {
            base.PlayTurnBase(CurrentBoard, turn);

            int[] userInput = GetUserInput(CurrentBoard);

            return new Turn(this, CurrentBoard, CurrentBoard.MakeMove(userInput[0], userInput[1], turn), userInput[0], userInput[1], turnNumber);
        }

        public static int[] GetUserInput(BoardPosition board)
        {
            while (true)
            {
                var UserInput = Console.ReadLine().Trim();

                if (Int32.TryParse(UserInput, out int userChoice) && board.IsFree(userChoice - 1))
                {
                    if (UserInput == "1") return new int[] { 0, 0 };
                    if (UserInput == "2") return new int[] { 0, 1 };
                    if (UserInput == "3") return new int[] { 0, 2 };
                    if (UserInput == "4") return new int[] { 1, 0 };
                    if (UserInput == "5") return new int[] { 1, 1 };
                    if (UserInput == "6") return new int[] { 1, 2 };
                    if (UserInput == "7") return new int[] { 2, 0 };
                    if (UserInput == "8") return new int[] { 2, 1 };
                    if (UserInput == "9") return new int[] { 2, 2 };
                }

                Console.WriteLine("Enter number between 1 and 9 that's available");
            }
        }
    }
}

