using Noughts_and_Crosses;

namespace Menace.ViewModels
{
    public class MenaceDetails
    {
        public Player Player { get; set; }
        public AIMenace menaceEngine { get; set; }
        public LinkedList<Matchbox> Matchboxes { get; set; }

        public TrainingHistory TrainingHistory { get; set; }
    }
}
