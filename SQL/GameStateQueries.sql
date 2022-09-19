--SELECT * FROM GameHistory

SELECT
	g.Id 'Game',
	t.TurnNumber,
	CASE
		WHEN t.TurnPlayerId = p1.Id THEN p1.Name
		ELSE p2.Name
	END 'Player',
	t.AfterBoardPositionId,
	t.X,
	t.Y
FROM GameHistory g
INNER JOIN Turn t ON g.Id = t.GameHistoryId
INNER JOIN Player p1 ON t.TurnPlayerId = p1.Id
INNER JOIN Player p2 ON t.TurnPlayerId = p2.Id
ORDER BY g.Id, t.TurnNumber

/*
SELECT *
FROM Matchbox m
INNER JOIN Bead b ON b.MatchboxId = m.Id
ORDER BY m.BoardPositionId, b.x, b.y
*/
