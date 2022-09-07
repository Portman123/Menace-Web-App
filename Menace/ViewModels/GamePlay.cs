namespace Menace.ViewModels
{
    public class GamePlay
    {
        public const string EmptyBoardPostion = "         ";

        public string Board { get; set; } = EmptyBoardPostion;

        public string CurrentPlayer { get; set; } = "X";

    }
}
