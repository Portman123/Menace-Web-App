﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class TrainingHistory : Entity
    {
        public List<TrainingRound> Rounds { get; set; } = new List<TrainingRound>();

        public TrainingRound AddRound(string opponentName)
        {
            var round = new TrainingRound
            {
                RoundNumber = Rounds.Count + 1,
                Opponent = opponentName
            };

            Rounds.Add(round);

            return round;
        }
    }
}