using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noughts_and_Crosses
{
    public class PlayerMenace : Player, IReinforcementLearner
    {
        public AIMenace MenaceEngine { get; set; }

        public TrainingHistory TrainingHistory { get; set; }

        [NotMapped]
        public TrainingRound CurrentTrainingRound { get; set; }

        public ReinforcementRewardFunction.RewardFunctionType ReinforcementType { get; set; }

        public PlayerMenace(string name) : base(name)
        {
            TrainingHistory = new TrainingHistory();
        }

        public PlayerMenace(AIMenace menaceEngine, string name, ReinforcementRewardFunction.RewardFunctionType rewardFunctionType) : base(name)
        {
            MenaceEngine = menaceEngine;
            TrainingHistory = new TrainingHistory();
            ReinforcementType = rewardFunctionType;
        }

        public TrainingRound StartTraining(string opponentName)
        {
            CurrentTrainingRound = TrainingHistory.AddRound(opponentName);

            return CurrentTrainingRound;
        }

        public TrainingRound StopTraining()
        {
            var round = CurrentTrainingRound;

            CurrentTrainingRound = null;

            return round;
        }

        private void LogTrainingRound(Game g)
        {
            if (CurrentTrainingRound == null) return;

            if (g.Winner == this)
            {
                CurrentTrainingRound.Wins++;
            }
            else if (g.Winner == null)
            {
                CurrentTrainingRound.Draws++;
            }
            else
            {
                CurrentTrainingRound.Losses++;
            }
        }

        public override Turn PlayTurn(BoardPosition CurrentBoard, int turn, int turnNumber)
        {
            int[] AIMove = MenaceEngine.PlayTurn(CurrentBoard, turn);

            return new Turn(this, CurrentBoard, CurrentBoard.MakeMove(AIMove[0], AIMove[1], turn), AIMove[0], AIMove[1], turnNumber);
        }

        public Turn PlayTurn(BoardPosition CurrentBoard)
        {
            var turn = CurrentBoard.TurnNumber % 2 == 0 ? -1 : 1;

            return PlayTurn(CurrentBoard, turn, CurrentBoard.TurnNumber);
        }

        public void Reinforce(Game g)
        {
            MenaceEngine.Reinforce(g, this);

            LogTrainingRound(g);
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
            Console.WriteLine(MenaceEngine.NumOfBeads());
        }
    }
}
