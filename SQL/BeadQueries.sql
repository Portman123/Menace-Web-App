SELECT b.X, b.Y, SUM(m.Wins) TotalWinsByPosition
FROM Player p
INNER JOIN AIMenace ai ON p.MenaceEngineId = ai.Id
INNER JOIN Matchbox m ON ai.Id = m.AIMenaceId
INNER JOIN Bead b ON b.MatchboxId = m.Id
WHERE
	p.Name = 'Player Menace 0'
	--AND TRIM(m.BoardPositionId) = 'O'
GROUP BY b.X, b.Y
ORDER BY SUM(m.Wins) DESC


