using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gremlin_eye.Server.Migrations
{
    /// <inheritdoc />
    public partial class V5SmootherIgdbDependencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSeries_game_series_SeriesId",
                table: "GameSeries");

            migrationBuilder.DropTable(
                name: "GameCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_game_series",
                table: "game_series");

            migrationBuilder.RenameTable(
                name: "game_series",
                newName: "series_data");

            migrationBuilder.AlterColumn<long>(
                name: "SeriesId",
                table: "GameSeries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "GameSeries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<long>(
                name: "PlatformId",
                table: "GamePlatforms",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "GamePlatforms",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<long>(
                name: "GenreId",
                table: "GameGenres",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "GameGenres",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_series_data",
                table: "series_data",
                column: "series_id");

            migrationBuilder.CreateTable(
                name: "GameCompanies",
                columns: table => new
                {
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCompanies", x => new { x.GameId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_GameCompanies_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameCompanies_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameCompanies_CompanyId",
                table: "GameCompanies",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSeries_series_data_SeriesId",
                table: "GameSeries",
                column: "SeriesId",
                principalTable: "series_data",
                principalColumn: "series_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSeries_series_data_SeriesId",
                table: "GameSeries");

            migrationBuilder.DropTable(
                name: "GameCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_series_data",
                table: "series_data");

            migrationBuilder.RenameTable(
                name: "series_data",
                newName: "game_series");

            migrationBuilder.AlterColumn<long>(
                name: "SeriesId",
                table: "GameSeries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "GameSeries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<long>(
                name: "PlatformId",
                table: "GamePlatforms",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "GamePlatforms",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<long>(
                name: "GenreId",
                table: "GameGenres",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "GameGenres",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_game_series",
                table: "game_series",
                column: "series_id");

            migrationBuilder.CreateTable(
                name: "GameCompany",
                columns: table => new
                {
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCompany", x => new { x.GameId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_GameCompany_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameCompany_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameCompany_CompanyId",
                table: "GameCompany",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSeries_game_series_SeriesId",
                table: "GameSeries",
                column: "SeriesId",
                principalTable: "game_series",
                principalColumn: "series_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
