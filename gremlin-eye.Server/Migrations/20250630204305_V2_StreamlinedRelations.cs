using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gremlin_eye.Server.Migrations
{
    /// <inheritdoc />
    public partial class V2_StreamlinedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_games_parent_id",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "FK_playthroughs_games_game_id",
                table: "playthroughs");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_games_game_id",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_reviews_game_id",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_playthroughs_game_id",
                table: "playthroughs");

            migrationBuilder.DropIndex(
                name: "IX_games_parent_id",
                table: "games");

            migrationBuilder.DropColumn(
                name: "game_id",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "game_id",
                table: "playthroughs");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "game_id",
                table: "reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "game_id",
                table: "playthroughs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "parent_id",
                table: "games",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reviews_game_id",
                table: "reviews",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_playthroughs_game_id",
                table: "playthroughs",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_games_parent_id",
                table: "games",
                column: "parent_id");

            migrationBuilder.AddForeignKey(
                name: "FK_games_games_parent_id",
                table: "games",
                column: "parent_id",
                principalTable: "games",
                principalColumn: "game_id");

            migrationBuilder.AddForeignKey(
                name: "FK_playthroughs_games_game_id",
                table: "playthroughs",
                column: "game_id",
                principalTable: "games",
                principalColumn: "game_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_games_game_id",
                table: "reviews",
                column: "game_id",
                principalTable: "games",
                principalColumn: "game_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
