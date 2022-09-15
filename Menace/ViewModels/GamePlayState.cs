namespace Menace.ViewModels
{
    public class GamePlayState
    {
        public const string EmptyBoardPostion = "         ";

        public string Board { get; set; } = EmptyBoardPostion;

        public string CurrentPlayer { get; set; } = "X";

        public Guid PlayerId1 { get; set; }

        public PlayerType Player1Type { get; set; }

        public Guid PlayerId2 { get; set; }

        public PlayerType Player2Type { get; set; }
    }
}
