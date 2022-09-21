using Noughts_and_Crosses;

namespace Menace.ViewModels
{
    public class GameCreate
    {
        public GameType Type { get; set; }

        public Guid Player1Id { get; set; }

        public Guid Player2Id { get; set; }
    }
}
