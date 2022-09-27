namespace Noughts_and_Crosses
{
    public static class AIOptimalMove
    {
        // this is a bit messy 
        public static readonly Coordinate TopLeft = new Coordinate(0, 0);
        public static readonly Coordinate TopMiddle = new Coordinate(0, 1);
        public static readonly Coordinate TopRight = new Coordinate(0, 2);

        public static readonly Coordinate MiddleLeft = new Coordinate(1, 0);
        public static readonly Coordinate Centre = new Coordinate(1, 1);
        public static readonly Coordinate MiddleRight = new Coordinate(1, 2);

        public static readonly Coordinate BottomLeft = new Coordinate(2, 0);
        public static readonly Coordinate BottomMiddle = new Coordinate(2, 1);
        public static readonly Coordinate BottomRight = new Coordinate(2, 2);

        public static readonly List<Coordinate> AllPositions = new List<Coordinate>
        {
            TopLeft,
            TopMiddle,
            TopRight,
            MiddleLeft,
            Centre,
            MiddleRight,
            BottomLeft,
            BottomMiddle,
            BottomRight
        };

        public static readonly WinningPosition TopRow = new WinningPosition(new Coordinate[] { TopLeft, TopMiddle, TopRight });
        public static readonly WinningPosition MiddleRow = new WinningPosition(new Coordinate[] { MiddleLeft, Centre, MiddleRight });
        public static readonly WinningPosition BottomRow = new WinningPosition(new Coordinate[] { BottomLeft, BottomMiddle, BottomRight });
        public static readonly WinningPosition LeftColumn = new WinningPosition(new Coordinate[] { TopLeft, MiddleLeft, BottomLeft });
        public static readonly WinningPosition MiddleColumn = new WinningPosition(new Coordinate[] { TopMiddle, Centre, BottomMiddle });
        public static readonly WinningPosition RightColumn = new WinningPosition(new Coordinate[] { TopRight, MiddleRight, BottomRight });
        public static readonly WinningPosition FirstDiagonal = new WinningPosition(new Coordinate[] { TopLeft, Centre, BottomRight });
        public static readonly WinningPosition SecondDiagonal = new WinningPosition(new Coordinate[] { BottomLeft, Centre, TopRight });

        public static readonly List<WinningPosition> WinningPositions = new List<WinningPosition>
        {
            TopRow,
            MiddleRow,
            BottomRow,
            LeftColumn,
            MiddleColumn,
            RightColumn,
            FirstDiagonal,
            SecondDiagonal
        };

        public static readonly List<WinningPositionIntersect> WinningIntersections = new List<WinningPositionIntersect>
        {
            new WinningPositionIntersect(TopRow, LeftColumn),
            new WinningPositionIntersect(TopRow, MiddleColumn),
            new WinningPositionIntersect(TopRow, RightColumn),

            new WinningPositionIntersect(MiddleRow, LeftColumn),
            new WinningPositionIntersect(MiddleRow, MiddleColumn),
            new WinningPositionIntersect(MiddleRow, RightColumn),

            new WinningPositionIntersect(BottomRow, LeftColumn),
            new WinningPositionIntersect(BottomRow, MiddleColumn),
            new WinningPositionIntersect(BottomRow, RightColumn),

            new WinningPositionIntersect(FirstDiagonal, TopRow),
            new WinningPositionIntersect(FirstDiagonal, MiddleRow),
            new WinningPositionIntersect(FirstDiagonal, BottomRow),
            new WinningPositionIntersect(FirstDiagonal, LeftColumn),
            new WinningPositionIntersect(FirstDiagonal, MiddleColumn),
            new WinningPositionIntersect(FirstDiagonal, RightColumn),

            new WinningPositionIntersect(SecondDiagonal, TopRow),
            new WinningPositionIntersect(SecondDiagonal, MiddleRow),
            new WinningPositionIntersect(SecondDiagonal, BottomRow),
            new WinningPositionIntersect(SecondDiagonal, LeftColumn),
            new WinningPositionIntersect(SecondDiagonal, MiddleColumn),
            new WinningPositionIntersect(SecondDiagonal, RightColumn),

            new WinningPositionIntersect(FirstDiagonal, SecondDiagonal)
        };

        public static readonly List<Coordinate> Corners = new List<Coordinate>
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        };

        public static readonly List<Coordinate> Sides = new List<Coordinate>
        {
            TopMiddle,
            BottomMiddle,
            MiddleLeft,
            MiddleRight
        };

        public static readonly List<(Coordinate, Coordinate)> OppositeCorners = new List<(Coordinate, Coordinate)>
        {
            (TopLeft, BottomRight),
            (BottomRight, TopLeft),
            (BottomLeft, TopRight),
            (TopRight, BottomLeft)
        };

        public static List<Coordinate> GetTiles(BoardPosition board, int player)
        {
            return AllPositions.FindAll(i => board.Occupant(i) == player);
        }

        public static int[] PickOne(List<Coordinate> choices)
        {
            Coordinate selectedCoord = choices.ElementAt(RandomNumberGenerator.Next(choices.Count));

            return new int[] { selectedCoord.X, selectedCoord.Y };
        }

        public static int[] PlayTurnTwo(Coordinate opponentTile)
        {
            if (Centre == opponentTile)
            {
                return PickOne(Corners);
            }

            if (Corners.Contains(opponentTile))
            {
                return new int[] { Centre.X, Centre.Y };
            }

            if (opponentTile == TopMiddle)
            {
                return PickOne(new List<Coordinate> { Centre, TopLeft, TopRight });
            }

            if (opponentTile == BottomMiddle)
            {
                return PickOne(new List<Coordinate> { Centre, BottomLeft, BottomRight });
            }

            if (opponentTile == MiddleLeft)
            {
                return PickOne(new List<Coordinate> { Centre, TopLeft, BottomLeft });
            }

            if (opponentTile == MiddleRight)
            {
                return PickOne(new List<Coordinate> { Centre, TopRight, BottomRight });
            }

            throw new Exception("Optimo: unhandled turn 2 scenario");
        }

        public static int[] PlayTurn(BoardPosition board, int player)
        {
            var turnNumber = board.TurnNumber;

            if (turnNumber == 1)
            {
                return PickOne(AllPositions);
            }

            var playerTiles = GetTiles(board, player);

            var opponent = -player;

            var opponentTiles = GetTiles(board, opponent);

            var emptyTiles = GetTiles(board, 0);

            if (turnNumber == 2)
            {
                return PlayTurnTwo(opponentTiles[0]);
            }

            // Try to win
            if (CanWin(board, player, out List<Coordinate> winMoves))
            {
                return PickOne(winMoves);
            }

            // Try to block
            if (CanWin(board, opponent, out List<Coordinate> blockWinMoves))
            {
                return PickOne(blockWinMoves);
            }

            if (turnNumber == 4 && playerTiles.Contains(Centre) && opponentTiles.Intersect(Corners).Count() == 2)
            {
                return PickOne(Sides);
            }

            // Try to fork
            if (CanFork(board, player, out List<Coordinate> forkMoves))
            {
                return PickOne(forkMoves);
            }

            // Try to fork
            if (CanFork(board, opponent, out List<Coordinate> blockForkMoves))
            {
                if (CanMake2InRow(board, player, blockForkMoves, out List<Coordinate> blockWithAttackMoves))
                {
                    return PickOne(blockWithAttackMoves);
                }
                else
                {
                    return PickOne(blockForkMoves);
                }
            }

            // Try Centre
            if (emptyTiles.Contains(Centre))
            {
                return new int[] { Centre.X, Centre.Y };
            }

            // Try Opposite Corner
            if (CanPlayOppositeCorner(board, player, out List<Coordinate> oppositeCornerMoves))
            {
                return PickOne(oppositeCornerMoves);
            }

            // Try Empty Corner
            if (CanPlayEmptyCorner(board, out List<Coordinate> emptyCornerMoves))
            {
                return PickOne(emptyCornerMoves);
            }

            // Play empty side
            if (CanPlayEmptySide(board, out List<Coordinate> emptySideMoves))
            {
                return PickOne(emptySideMoves);
            }

            // if can't play good move return random move from those available
            throw new Exception("Optimo couldn't figure out a good move to play so he gave up :(");
        }

        // WIN
        // If there is a row, column, or diagonal with two of my pieces and a blank space,
        //      Then play the blank space (thus winning the game). 
        public static bool CanWin(BoardPosition boardPos, int player, out List<Coordinate> possibleMoves)
        {
            possibleMoves = new List<Coordinate>();

            // Determine the winning coordinate
            foreach (var winPos in WinningPositions)
            {
                // Work out if there are any winning positions which can be completed
                int progress = 0;
                Coordinate winCoord = null;
                for (int coord = 0; coord < 3; coord++)
                {
                    if (boardPos.Occupant(winPos.Coordinates[coord]) == player) progress++;
                    else if (boardPos.Occupant(winPos.Coordinates[coord]) == 0) winCoord = winPos.Coordinates[coord]; // take note of which coordinate was available
                }
                if (progress == 2 && winCoord != null) possibleMoves.Add(winCoord);
            }

            return possibleMoves.Count > 0;
        }

        // FORK
        // If there are two intersecting rows, columns, or diagonals with one of my pieces and two blanks, and
        //  If the intersecting space Is empty,
        //      Then move to the intersecting space (thus creating two ways to win on my next turn).
        public static bool CanFork(BoardPosition boardPos, int player, out List<Coordinate> possibleMoves)
        {
            possibleMoves = new List<Coordinate>();

            foreach (var Intersect in WinningIntersections)
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
                    possibleMoves.Add(Intersect.IntersectCoord);
                }
            }

            return possibleMoves.Count > 0;
        }

        public static bool CanMake2InRow(BoardPosition boardPos, int player, List<Coordinate> intersectingCoords, out List<Coordinate> possibleMoves)
        {
            possibleMoves = new List<Coordinate>();

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
                    if (filled == 1 && empty == 2 && move != null) possibleMoves.Add(interCord);
                }
            }

            return possibleMoves.Count > 0;
        }

        // PLAY OPPOSITE CORNER
        // If my opponent is in a corner, and If the opposite corner is empty,
        //      THEN play the opposite corner.
        public static bool CanPlayOppositeCorner(BoardPosition boardPos, int player, out List<Coordinate> possibleMoves)
        {
            possibleMoves = new List<Coordinate>();

            foreach ((Coordinate, Coordinate) cornerPair in OppositeCorners)
            {
                if (boardPos.Occupant(cornerPair.Item1) == player * -1 && boardPos.Occupant(cornerPair.Item2) == 0)
                {
                    possibleMoves.Add(cornerPair.Item2);
                }
            }

            return possibleMoves.Count > 0;
        }

        // PLAY EMPTY CORNER
        // If there is an empty corner, Then move to an empty corner.
        public static bool CanPlayEmptyCorner(BoardPosition boardPos, out List<Coordinate> possibleMoves)
        {
            possibleMoves = new List<Coordinate>();

            foreach (Coordinate corner in Corners)
            {
                if (boardPos.Occupant(corner) == 0) possibleMoves.Add(corner);
            }

            return possibleMoves.Count > 0;
        }

        // PLAY EMPTY SIDE
        // If there Is an empty side, Then move to an empty side.
        public static bool CanPlayEmptySide(BoardPosition boardPos, out List<Coordinate> possibleMoves)
        {
            possibleMoves = new List<Coordinate>();

            foreach (Coordinate side in Sides)
            {
                if (boardPos.Occupant(side) == 0) possibleMoves.Add(side);
            }

            return possibleMoves.Count > 0;
        }
    }
}
