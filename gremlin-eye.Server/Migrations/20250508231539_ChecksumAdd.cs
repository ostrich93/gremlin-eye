using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gremlin_eye.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChecksumAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "checksum",
                table: "platforms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "checksum",
                table: "genres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "checksum",
                table: "games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "checksum",
                table: "game_series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "checksum",
                table: "companies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "checksum",
                table: "platforms");

            migrationBuilder.DropColumn(
                name: "checksum",
                table: "genres");

            migrationBuilder.DropColumn(
                name: "checksum",
                table: "games");

            migrationBuilder.DropColumn(
                name: "checksum",
                table: "game_series");

            migrationBuilder.DropColumn(
                name: "checksum",
                table: "companies");
        }
    }
}
