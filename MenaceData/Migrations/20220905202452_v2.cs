using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenaceData.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Encoded",
                table: "BoardPosition",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Encoded",
                table: "BoardPosition");

            migrationBuilder.DropColumn(
                name: "isFullBoard",
                table: "BoardPosition");

            migrationBuilder.DropColumn(
                name: "isWinningPosition",
                table: "BoardPosition");
        }
    }
}
