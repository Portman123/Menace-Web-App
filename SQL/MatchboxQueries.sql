SELECT REPLACE('[' + m.BoardPositionId + ']', ' ',  '.'), m.Wins + m.Draws + m.Losses as 'Games', m.Wins, m.Draws, m.Losses, b.X, b.Y, b.Count
FROM Player p
INNER JOIN AIMenace ai ON p.MenaceEngineId = ai.Id
INNER JOIN Matchbox m ON ai.Id = m.AIMenaceId
INNER JOIN Bead b ON b.MatchboxId = m.Id
WHERE
	p.Name = 'Player Menace 0'
	--AND TRIM(m.BoardPositionId) = 'O'
ORDER BY m.Wins + m.Draws + m.Losses DESC, b.X, b.Y
