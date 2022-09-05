using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noughts_and_Crosses
{
    public class AIRandomMove : Entity, IAIEngine
    {
        public int[] PlayTurn(BoardPosition boardPos, int turn)
        {
            LinkedList<int[]> available = DetermineAvailableMoves(boardPos);

            // return a random move from those available
            return available.ElementAt(RandomNumberGenerator.Next(available.Count));
        }

        public LinkedList<int[]> DetermineAvailableMoves(BoardPosition boardPos)
        {
            // return a list of available moves to be made
            LinkedList<int[]> available = new LinkedList<int[]>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (boardPos.Coords[i, j] == 0)
                    {
                        available.AddLast(new int[] {i,j});
                    }
                }
            }
            return available;
        }
    }
}
