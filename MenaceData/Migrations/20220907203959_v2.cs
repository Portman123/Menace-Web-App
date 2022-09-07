using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenaceData.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnNumber",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "isFullBoard",
                table: "BoardPosition");

            migrationBuilder.DropColumn(
                name: "isWinningPosition",
                table: "BoardPosition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TurnNumber",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isFullBoard",
                table: "BoardPosition",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isWinningPosition",
                table: "BoardPosition",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
