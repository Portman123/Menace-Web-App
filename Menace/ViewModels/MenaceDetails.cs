using Noughts_and_Crosses;
using System.ComponentModel.DataAnnotations;

namespace Menace.ViewModels
{
    public class MenaceDetails
    {
        public Guid PlayerId { get; set; }

        public Player Player { get; set; }

        [Display(Name = "Reward Function")]
        public string RewardFunctionType { get; set; }

        public AIMenace menaceEngine { get; set; }

        public LinkedList<Matchbox> Matchboxes { get; set; }

        public TrainingHistory TrainingHistory { get; set; }

        public string TrainingOption { get; set; }

        public int NumberOfTrainingGames { get; set; }
    }
}
