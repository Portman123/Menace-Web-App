IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [BoardPosition] (
    [Id] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_BoardPosition] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Menace] (
    [Id] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Menace] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Matchbox] (
    [Id] uniqueidentifier NOT NULL,
    [BoardPositionId] uniqueidentifier NOT NULL,
    [AIMenaceId] uniqueidentifier NULL,
    CONSTRAINT [PK_Matchbox] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Matchbox_BoardPosition_BoardPositionId] FOREIGN KEY ([BoardPositionId]) REFERENCES [BoardPosition] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Matchbox_Menace_AIMenaceId] FOREIGN KEY ([AIMenaceId]) REFERENCES [Menace] ([Id])
);
GO

CREATE TABLE [Bead] (
    [Id] uniqueidentifier NOT NULL,
    [X] int NOT NULL,
    [Y] int NOT NULL,
    [Count] int NOT NULL,
    [MatchboxId] uniqueidentifier NULL,
    CONSTRAINT [PK_Bead] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bead_Matchbox_MatchboxId] FOREIGN KEY ([MatchboxId]) REFERENCES [Matchbox] ([Id])
);
GO

CREATE INDEX [IX_Bead_MatchboxId] ON [Bead] ([MatchboxId]);
GO

CREATE INDEX [IX_Matchbox_AIMenaceId] ON [Matchbox] ([AIMenaceId]);
GO

CREATE INDEX [IX_Matchbox_BoardPositionId] ON [Matchbox] ([BoardPositionId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220903173101_initial migration', N'6.0.8');
GO

COMMIT;
GO

