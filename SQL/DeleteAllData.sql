USE [Menace]

DELETE Game
DELETE Turn
DELETE GameHistory
DELETE Player WHERE Name <> 'Human'
DELETE Bead
DELETE Matchbox
DELETE AIMenace
--DELETE AIOptimalMove
--DELETE AIRandomMove
DELETE BoardPosition WHERE BoardPositionId <> '         '
--DELETE __EFMigrationsHistory
