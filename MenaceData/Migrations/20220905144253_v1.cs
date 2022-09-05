﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenaceData.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AIMenace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIMenace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AIOptimalMove",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIOptimalMove", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AIRandomMove",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIRandomMove", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoardPosition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardPosition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wins = table.Column<int>(type: "int", nullable: false),
                    Draws = table.Column<int>(type: "int", nullable: false),
                    Losses = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenaceEngineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OptimalEngineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RandomEngineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_AIMenace_MenaceEngineId",
                        column: x => x.MenaceEngineId,
                        principalTable: "AIMenace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Player_AIOptimalMove_OptimalEngineId",
                        column: x => x.OptimalEngineId,
                        principalTable: "AIOptimalMove",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Player_AIRandomMove_RandomEngineId",
                        column: x => x.RandomEngineId,
                        principalTable: "AIRandomMove",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matchbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AIMenaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matchbox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matchbox_AIMenace_AIMenaceId",
                        column: x => x.AIMenaceId,
                        principalTable: "AIMenace",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matchbox_BoardPosition_BoardPositionId",
                        column: x => x.BoardPositionId,
                        principalTable: "BoardPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turn_GameHistories_GameHistoryId",
                        column: x => x.GameHistoryId,
                        principalTable: "GameHistories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    P1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    P2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TurnNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_BoardPosition_CurrentBoardId",
                        column: x => x.CurrentBoardId,
                        principalTable: "BoardPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Player_P1Id",
                        column: x => x.P1Id,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Games_Player_P2Id",
                        column: x => x.P2Id,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Games_Player_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bead",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    MatchboxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bead", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bead_Matchbox_MatchboxId",
                        column: x => x.MatchboxId,
                        principalTable: "Matchbox",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bead_MatchboxId",
                table: "Bead",
                column: "MatchboxId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CurrentBoardId",
                table: "Games",
                column: "CurrentBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_P1Id",
                table: "Games",
                column: "P1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_P2Id",
                table: "Games",
                column: "P2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WinnerId",
                table: "Games",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Matchbox_AIMenaceId",
                table: "Matchbox",
                column: "AIMenaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Matchbox_BoardPositionId",
                table: "Matchbox",
                column: "BoardPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_MenaceEngineId",
                table: "Player",
                column: "MenaceEngineId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_OptimalEngineId",
                table: "Player",
                column: "OptimalEngineId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_RandomEngineId",
                table: "Player",
                column: "RandomEngineId");

            migrationBuilder.CreateIndex(
                name: "IX_Turn_GameHistoryId",
                table: "Turn",
                column: "GameHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bead");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Turn");

            migrationBuilder.DropTable(
                name: "Matchbox");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "GameHistories");

            migrationBuilder.DropTable(
                name: "BoardPosition");

            migrationBuilder.DropTable(
                name: "AIMenace");

            migrationBuilder.DropTable(
                name: "AIOptimalMove");

            migrationBuilder.DropTable(
                name: "AIRandomMove");
        }
    }
}
