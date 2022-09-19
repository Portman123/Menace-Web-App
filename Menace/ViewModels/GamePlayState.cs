namespace Menace.ViewModels
{
    public class GamePlayState
    {
        public static string WrapBoard(string board) => $"[{board}]";

        public static string UnwrapBoard(string board) => board.Substring(1, 9);

        public string BoardBeforeInput { get; set; }

        public string BoardAfterInput { get; set; }

        public string CurrentPlayerSymbol { get; set; } = "X";

        public Guid GameHistoryId { get; set; }

        public bool IsGameActive { get; set; }

        //public Guid PlayerId1 { get; set; }

        //public PlayerType Player1Type { get; set; }

        //public Guid PlayerId2 { get; set; }

        //public PlayerType Player2Type { get; set; }
    }
}
