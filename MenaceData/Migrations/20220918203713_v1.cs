using System;
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
                    BoardPositionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardPosition", x => x.BoardPositionId);
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
                    BoardPositionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                        principalColumn: "BoardPositionId");
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentBoardBoardPositionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    P1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    P2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_BoardPosition_CurrentBoardBoardPositionId",
                        column: x => x.CurrentBoardBoardPositionId,
                        principalTable: "BoardPosition",
                        principalColumn: "BoardPositionId");
                    table.ForeignKey(
                        name: "FK_Game_Player_P1Id",
                        column: x => x.P1Id,
                        principalTable: "Player",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Game_Player_P2Id",
                        column: x => x.P2Id,
                        principalTable: "Player",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    P1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    P2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameHistory_Player_P1Id",
                        column: x => x.P1Id,
                        principalTable: "Player",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameHistory_Player_P2Id",
                        column: x => x.P2Id,
                        principalTable: "Player",
                        principalColumn: "Id");
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

            migrationBuilder.CreateTable(
                name: "Turn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TurnPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BeforeBoardPositionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AfterBoardPositionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    TurnNumber = table.Column<int>(type: "int", nullable: false),
                    GameHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turn_BoardPosition_AfterBoardPositionId",
                        column: x => x.AfterBoardPositionId,
                        principalTable: "BoardPosition",
                        principalColumn: "BoardPositionId");
                    table.ForeignKey(
                        name: "FK_Turn_BoardPosition_BeforeBoardPositionId",
                        column: x => x.BeforeBoardPositionId,
                        principalTable: "BoardPosition",
                        principalColumn: "BoardPositionId");
                    table.ForeignKey(
                        name: "FK_Turn_GameHistory_GameHistoryId",
                        column: x => x.GameHistoryId,
                        principalTable: "GameHistory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Turn_Player_TurnPlayerId",
                        column: x => x.TurnPlayerId,
                        principalTable: "Player",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "BoardPosition",
                column: "BoardPositionId",
                value: "         ");

            migrationBuilder.CreateIndex(
                name: "IX_Bead_MatchboxId",
                table: "Bead",
                column: "MatchboxId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CurrentBoardBoardPositionId",
                table: "Game",
                column: "CurrentBoardBoardPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_P1Id",
                table: "Game",
                column: "P1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Game_P2Id",
                table: "Game",
                column: "P2Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameHistory_P1Id",
                table: "GameHistory",
                column: "P1Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameHistory_P2Id",
                table: "GameHistory",
                column: "P2Id");

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
                name: "IX_Turn_AfterBoardPositionId",
                table: "Turn",
                column: "AfterBoardPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Turn_BeforeBoardPositionId",
                table: "Turn",
                column: "BeforeBoardPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Turn_GameHistoryId",
                table: "Turn",
                column: "GameHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Turn_TurnPlayerId",
                table: "Turn",
                column: "TurnPlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bead");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Turn");

            migrationBuilder.DropTable(
                name: "Matchbox");

            migrationBuilder.DropTable(
                name: "GameHistory");

            migrationBuilder.DropTable(
                name: "BoardPosition");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "AIMenace");

            migrationBuilder.DropTable(
                name: "AIOptimalMove");

            migrationBuilder.DropTable(
                name: "AIRandomMove");
        }
    }
}
