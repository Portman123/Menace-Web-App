using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class Game : Entity
    {
        public BoardPosition CurrentBoard { get; set; }

        public GameHistory History { get; }

        public bool Finished { get; set; }

        public Player P1 { get; set; }
        [NotMapped]
        public IReinforcementLearner P1Learner { get; set; }

        public Player P2 { get; set; }
        [NotMapped]
        public IReinforcementLearner P2Learner { get; set; }

        [NotMapped]
        public Player Winner { get; set; }

        public Game()
        {
        }
        public Game(Player p1, Player p2)
        {
            CurrentBoard = new BoardPosition();
            P1 = p1;
            P1Learner = p1 as IReinforcementLearner;
            P2 = p2;
            P2Learner = p2 as IReinforcementLearner;
            History = new GameHistory(P1, P2);
            Finished = false;
        }

        public void PlayGameOnConsole()
        {
            if (Finished) throw new Exception("Cannot play a game that has already Finished!");

            // Game Loop
            while (true)
            {
                // Player 1 turn
                Turn p1Turn = P1.PlayTurn(CurrentBoard, GameSymbol.MapSymbolToInt("X"), CurrentBoard.TurnNumber);
                CurrentBoard = p1Turn.After;
                History.AddMove(p1Turn);
                CurrentBoard.PrintBoard();

                // Check Game End
                if (CurrentBoard.IsGameOver) break;

                // Player 2 Turn 
                Turn p2Turn = P2.PlayTurn(CurrentBoard, GameSymbol.MapSymbolToInt("O"), CurrentBoard.TurnNumber);
                CurrentBoard = p2Turn.After;
                History.AddMove(p2Turn);
                CurrentBoard.PrintBoard();

                // Check Game End
                if (CurrentBoard.IsGameOver) break;
            }
            CurrentBoard.PrintBoard();
            AnnounceWinner();
            Finished = true;
            DetermineWinner();

            // Apply Reinforcements
            if (P1Learner != null) P1Learner.Reinforce(this);

            if (P2Learner != null) P2Learner.Reinforce(this);
        }

        public void Train()
        {
            if (Finished) throw new Exception("Cannot play a game that has already Finished!");

            // Game Loop
            while (true)
            {
                // Player 1 turn
                Turn p1Turn = P1.PlayTurn(CurrentBoard, GameSymbol.MapSymbolToInt("X"), CurrentBoard.TurnNumber);

                CurrentBoard = p1Turn.After;

                History.AddMove(p1Turn);

                // Check Game End
                if (p1Turn.After.IsGameOver) break;

                // Player 2 Turn 
                Turn p2Turn = P2.PlayTurn(p1Turn.After, GameSymbol.MapSymbolToInt("O"), p1Turn.After.TurnNumber);

                CurrentBoard = p2Turn.After;

                History.AddMove(p2Turn);

                // Check Game End
                if (p2Turn.After.IsGameOver) break;
            }

            Finished = true;

            DetermineWinner();

            //AnnounceWinner();

            // Apply Reinforcements
            if (P1Learner != null) P1Learner.Reinforce(this);

            if (P2Learner != null) P2Learner.Reinforce(this);
        }

        public void AnnounceWinner()
        {
            // Write winner to console
            if (CurrentBoard.CheckWin() == GameSymbol.MapSymbolToInt("X"))
            {
                Console.WriteLine("");
                Console.Write(P1.Name);
                Console.Write(" wins!");
            }
            else if (CurrentBoard.CheckWin() == GameSymbol.MapSymbolToInt("O"))
            {
                Console.WriteLine("");
                Console.Write(P2.Name);
                Console.Write(" wins!");
            }
            else if (CurrentBoard.CheckWin() == 0) Console.WriteLine("Draw!");
            else Console.WriteLine("Something has gone wrong...");
        }

        public void DetermineWinner()
        {
            // Write winner to console
            if (CurrentBoard.CheckWin() == GameSymbol.MapSymbolToInt("X"))
            {
                Winner = P1;
                P1.Wins++;
                P2.Losses++;
            }
            else if (CurrentBoard.CheckWin() == GameSymbol.MapSymbolToInt("O"))
            {
                Winner = P2;
                P2.Wins++;
                P1.Losses++;
            }
            else if (CurrentBoard.CheckWin() == 0)
            {
                Winner = null;
                P1.Draws++;
                P2.Draws++;
            }
            else Console.WriteLine("Something has gone wrong...");
        }

        public Player ReturnWinner()
        {
            return Winner;
        }
    }
}
