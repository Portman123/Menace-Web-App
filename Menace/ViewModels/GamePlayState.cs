using Noughts_and_Crosses;

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

        public GameType GameType { get; set; }

        public LinkedList<Bead>? Beads { get; set; }


        public Matchbox? Matchbox { get; set; }

        //public Matchbox currentMatchbox { get; set; }

        public Guid Player1Id { get; set; }

        //public PlayerType Player1Type { get; set; }

        public Guid Player2Id { get; set; }

        public string? MenaceName { get; set; }

        //public PlayerType Player2Type { get; set; }
    }
}
