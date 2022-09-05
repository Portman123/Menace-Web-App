using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Noughts_and_Crosses
{
    public class AIOptimalMove : IAIEngine
    {
        // this is a bit messy but techinically this could be adapted to play on more than just a 3x3 grid? not sure but it works for now
        public LinkedList<WinningPosition> WinningPositions = new LinkedList<WinningPosition> ();
        public LinkedList<(Coordinate, Coordinate)> Corners = new LinkedList<(Coordinate, Coordinate)>();
        public LinkedList<Coordinate> Sides = new LinkedList<Coordinate> ();
        public LinkedList<WinningPositionIntersect> Intersections { get; }

        public AIOptimalMove()
        {
            // While I could have written code to automatically work this out, I decided it was more readable and quicker to write this way
            WinningPosition topRow = new WinningPosition(new Coordinate[] { new Coordinate(0, 0), new Coordinate(0, 1), new Coordinate(0, 2)});
            WinningPosition middleRow = new WinningPosition(new Coordinate[] { new Coordinate(1, 0), new Coordinate(1, 1), new Coordinate(1, 2) });
            WinningPosition bottomRow = new WinningPosition(new Coordinate[] { new Coordinate(2, 0), new Coordinate(2, 1), new Coordinate(2, 2) });
            WinningPosition leftColumn = new WinningPosition(new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 0), new Coordinate(2, 0) });
            WinningPosition middleColumn = new WinningPosition(new Coordinate[] { new Coordinate(0, 1), new Coordinate(1, 1), new Coordinate(2, 1) });
            WinningPosition rightColumn = new WinningPosition(new Coordinate[] { new Coordinate(0, 2), new Coordinate(1, 2), new Coordinate(2, 2) });
            WinningPosition firstDiagonal = new WinningPosition(new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2) });
            WinningPosition secondDiagonal = new WinningPosition(new Coordinate[] { new Coordinate(0, 2), new Coordinate(1, 1), new Coordinate(2, 0) });
            
            WinningPositions.AddLast(topRow);
            WinningPositions.AddLast(middleRow);
            WinningPositions.AddLast(bottomRow);
            WinningPositions.AddLast(leftColumn);
            WinningPositions.AddLast(middleColumn);
            WinningPositions.AddLast(rightColumn);
            WinningPositions.AddLast(firstDiagonal);
            WinningPositions.AddLast(secondDiagonal);

            // Make the Coordinates
            Corners.AddLast((new Coordinate(0, 0), new Coordinate(2,2)));
            Corners.AddLast((new Coordinate(0, 2), new Coordinate(2,0)));
            Corners.AddLast((new Coordinate(2, 0), new Coordinate(0,2)));
            Corners.AddLast((new Coordinate(2, 2), new Coordinate(0,0)));

            Sides.AddLast(new Coordinate(0, 1));
            Sides.AddLast(new Coordinate(1, 0));
            Sides.AddLast(new Coordinate(1, 2));
            Sides.AddLast(new Coordinate(2, 1));

            // Determine which winning positions intersect 
            Intersections = DetermineIntersections();
        }



        public int[] PlayTurn(BoardPosition boardPos, int turn)
        {
            Coordinate winMove = Win(boardPos, turn);
            Coordinate blockMove = Block(boardPos, turn);
            Coordinate forkMove = Fork(boardPos, turn);
            Coordinate blockForkMove = BlockFork(boardPos, turn);
            Coordinate centreMove = PlayCentre(boardPos);
            Coordinate oppositeCornerMove = PlayOppositeCorner(boardPos, turn);
            Coordinate emptyCornerMove = PlayEmptyCorner(boardPos);
            Coordinate emptySideMove = PlayEmptySide(boardPos);

            if (winMove != null) return new int[] { winMove.X, winMove.Y };// Try to win
            if (blockMove != null) return new int[] { blockMove.X, blockMove.Y };// Try to block
            if (forkMove != null) return new int[] { forkMove.X, forkMove.Y }; // Try to fork
            if (blockForkMove != null) return new int[] { blockForkMove.X, blockForkMove.Y }; // Try to block a fork
            if (centreMove != null) return new int[] { centreMove.X, centreMove.Y };
            if (oppositeCornerMove != null) return new int[] {oppositeCornerMove.X, oppositeCornerMove.Y };
            if (emptyCornerMove != null) return new int[] { emptyCornerMove.X, emptyCornerMove.Y };
            if (emptySideMove != null) return new int[] { emptySideMove.X, emptySideMove.Y };

            // if can't play good move return random move from those available
            throw new Exception("Optimo couldn't figure out a good move to play so he gave up :(");
            //LinkedList<Coordinate> available = DetermineAvailableMoves(boardPos);
            //Coordinate move = available.ElementAt(RandomNumberGenerator.Next(available.Count));
            //return new int[] { move.X, move.Y };
        }


        // WIN
        // If there is o row, column, or diagonal with two of my pieces and a blank space,
        //      Then play the blank space (thus winning the game). 
        public Coordinate Win(BoardPosition boardPos, int player)
        {
            // Determine the winning coordinate
            foreach (var winPos in WinningPositions)
            {
                // Work out if there are any winning positions which can be completed
                int progress = 0;
                Coordinate winCoord = null;
                for(int coord = 0; coord < 3; coord++)
                {
                    if (boardPos.Occupant(winPos.Coordinates[coord]) == player) progress++;
                    else if (boardPos.Occupant(winPos.Coordinates[coord]) == 0) winCoord = winPos.Coordinates[coord]; // take note of which coordinate was available
                }
                if (progress == 2 && winCoord != null) return winCoord;
            }
            return null;
        }


        // BLOCK
        // If there Is a row, column, or diagonal with two of my opponent’s pieces and a blank space,
        //      Then play the blank space (thus blocking a potential win for my opponent).
        public Coordinate Block(BoardPosition boardPos, int player)
        {
            // Determine opponents winning coordinate
            foreach (var winPos in WinningPositions)
            {
                // Work out if there are any winning positions which can be completed by opponent
                int progress = 0;
                Coordinate winCoord = null;
                for (int coord = 0; coord < 3; coord++)
                {
                    if (boardPos.Occupant(winPos.Coordinates[coord]) == player*-1) progress++;
                    else if (boardPos.Occupant(winPos.Coordinates[coord]) == 0) winCoord = winPos.Coordinates[coord]; // take note of which coordinate was available
                }
                if (progress == 2 && winCoord != null) return winCoord;
            }
            return null;
        }


        // FORK
        // If there are two intersecting rows, columns, or diagonals with one of my pieces and two blanks, and
        //  If the intersecting space Is empty,
        //      Then move to the intersecting space (thus creating two ways to win on my next turn).
        public Coordinate Fork(BoardPosition boardPos, int player)
        {
            foreach (var Intersect in Intersections)
            {
                // check if pos1 meets criteria
                int empty1 = 0;
                int filled1 = 0;
                foreach (Coordinate i in Intersect.WinPos1.Coordinates)
                {
                    if (boardPos.Occupant(i) == player) filled1++;
                    if (boardPos.Occupant(i) == 0) empty1++;
                }

                // check if pos2 meets criteria
                int empty2 = 0;
                int filled2 = 0;
                foreach (Coordinate i in Intersect.WinPos2.Coordinates)
                {
                    if (boardPos.Occupant(i) == player) filled2++;
                    if (boardPos.Occupant(i) == 0) empty2++;
                }

                // if criteria met, and intersecting coord empty return it 
                if (empty1 == 2 && filled1 == 1 && empty2 == 2 && filled2 == 1 && boardPos.Occupant(Intersect.IntersectCoord) == 0)
                {
                    return (Intersect.IntersectCoord);
                }
            }
            return null;
        }

        // BLOCK FORK
        // If there are two intersecting rows, columns, or diagonals with one of my opponent’s pieces ond two blanks, and
        //  If the intersecting space is empty,
        //      THEN
        //      If there is an empty location that creates a two-in-a-row for me (thus forcing my opponent to block rather than fork),
        //          THEN move to the location. 
        //      ELSE
        //          Move to the Intersection space (thus occupying the location that my opponent could use to fork). 
        public Coordinate BlockFork(BoardPosition boardPos, int player)
        {
            LinkedList<Coordinate> intersectingCoords = new LinkedList<Coordinate>();
            foreach (var Intersect in Intersections)
            {
                // check if pos1 meets criteria
                int empty1 = 0;
                int filled1 = 0;
                foreach (Coordinate i in Intersect.WinPos1.Coordinates)
                {
                    if (boardPos.Occupant(i) == player*-1) filled1++;
                    if (boardPos.Occupant(i) == 0) empty1++;
                }

                // check if pos2 meets criteria
                int empty2 = 0;
                int filled2 = 0;
                foreach (Coordinate i in Intersect.WinPos2.Coordinates)
                {
                    if (boardPos.Occupant(i) == player*-1) filled2++;
                    if (boardPos.Occupant(i) == 0) empty2++;
                }

                // if criteria met, and intersecting coord empty...
                if (empty1 == 2 && filled1 == 1 && empty2 == 2 && filled2 == 1 && boardPos.Occupant(Intersect.IntersectCoord) == 0)
                {
                    // add to list of potential forks to block
                    intersectingCoords.AddLast(Intersect.IntersectCoord);
                }
            }

            // try to get 2 in a row
            Coordinate move = Make2InRow(boardPos, player, intersectingCoords);
            if (move != null) return move; // HORRIBLE (definitely needs rewriting)
            else if (intersectingCoords.Count > 0) return intersectingCoords.First(); // Otherwise block opponents fork

            return null;
        }

        public Coordinate Make2InRow(BoardPosition boardPos, int player, LinkedList<Coordinate> intersectingCoords)
        {
            if (intersectingCoords == null) return null;
            // for each winning position check...
            foreach (WinningPosition position in WinningPositions)
            {
                foreach (Coordinate interCord in intersectingCoords)
                {
                    int empty = 0;
                    int filled = 0;
                    Coordinate move = null;
                    // how many of the coords are empty or filled with this players token
                    foreach (Coordinate coord in position.Coordinates)
                    {
                        if (boardPos.Occupant(coord) == player) filled++;
                        if (boardPos.Occupant(coord) == 0 && coord.Equals(interCord)) move = interCord; // This is horrible
                        if (boardPos.Occupant(coord) == 0) empty++;
                    }
                    // if conditions are right and a blocking cord also making 2 in a row is found then return it.
                    if (filled == 1 && empty == 2 && move != null) return interCord;
                }
            }
            return null;
        }

        // PLAY CENTRE
        // If the center is blank, Then play the center
        public Coordinate PlayCentre(BoardPosition boardPos)
        {
            Coordinate centre = new Coordinate(1, 1);
            if (boardPos.Occupant(new Coordinate(1, 1)) == 0) return centre;
            return null;
        }

        // PLAY OPPOSITE CORNER
        // If my opponent is in a corner, and If the opposite corner is empty,
        //      THEN play the opposite corner.
        public Coordinate PlayOppositeCorner(BoardPosition boardPos, int player)
        {
            foreach ((Coordinate,Coordinate) cornerPair in Corners)
            {
                if (boardPos.Occupant(cornerPair.Item1) == player*-1 && boardPos.Occupant(cornerPair.Item2)==0)
                {
                    return cornerPair.Item2;
                }
            }
            return null;
        }

        // PLAY EMPTY CORNER
        // If there is an empty corner, Then move to an empty corner.
        public Coordinate PlayEmptyCorner(BoardPosition boardPos)
        {
            foreach ((Coordinate, Coordinate) cornerPair in Corners)
            {
                if (boardPos.Occupant(cornerPair.Item1) == 0) return cornerPair.Item1;
            }
            return null;
        }

        // PLAY EMPTY SIDE
        // If there Is an empty side, Then move to an empty side.
        public Coordinate PlayEmptySide(BoardPosition boardPos)
        {
            foreach (Coordinate side in Sides)
            {
                if (boardPos.Occupant(side) == 0) return side;
            }
            return null;
        }

        // Create a record of the Intersecting winning positions
        public LinkedList<WinningPositionIntersect> DetermineIntersections()
        {
            LinkedList <WinningPositionIntersect> inters = new LinkedList<WinningPositionIntersect>();
            foreach (WinningPosition position1 in WinningPositions)
            {
                foreach (WinningPosition position2 in WinningPositions)
                {
                    WinningPositionIntersect temp = new WinningPositionIntersect(position1, position2);
                    if (position1.Intersects(position2) && !inters.Any(n => n.Equals(temp))) inters.AddLast(new WinningPositionIntersect(position1, position2));
                }
            }
            return inters;
        }

        public LinkedList<Coordinate> DetermineAvailableMoves(BoardPosition boardPos)
        {
            // return a list of available moves to be made
            LinkedList<Coordinate> available = new LinkedList<Coordinate>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (boardPos.Coords[i, j] == 0)
                    {
                        available.AddLast(new Coordinate( i, j ));
                    }
                }
            }
            return available;
        }
    }
}
