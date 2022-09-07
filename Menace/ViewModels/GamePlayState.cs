namespace Menace.ViewModels
{
    public class GamePlayState
    {
        public const string EmptyBoardPostion = "         ";

        public string Board { get; set; } = EmptyBoardPostion;

        public string CurrentPlayer { get; set; } = "X";

        public Guid BoardPositionId { get; set; }

        public Guid PlayerId1 { get; set; }

        public Guid PlayerId2 { get; set; } 
    }
}
