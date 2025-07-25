using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gremlin_eye.Server.Migrations
{
    /// <inheritdoc />
    public partial class V3_FixedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "game_id",
                table: "playthroughs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_playthroughs_game_id",
                table: "playthroughs",
                column: "game_id");

            migrationBuilder.AddForeignKey(
                name: "FK_playthroughs_games_game_id",
                table: "playthroughs",
                column: "game_id",
                principalTable: "games",
                principalColumn: "game_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_playthroughs_games_game_id",
                table: "playthroughs");

            migrationBuilder.DropIndex(
                name: "IX_playthroughs_game_id",
                table: "playthroughs");

            migrationBuilder.DropColumn(
                name: "game_id",
                table: "playthroughs");
        }
    }
}
