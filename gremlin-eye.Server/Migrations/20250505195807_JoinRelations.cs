using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gremlin_eye.Server.Migrations
{
    /// <inheritdoc />
    public partial class JoinRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "CompanyDataGameData");

            migrationBuilder.DropTable(
                name: "GameDataGenreData");

            migrationBuilder.DropTable(
                name: "GameDataPlatformData");

            migrationBuilder.DropTable(
                name: "GameDataSeries");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RefreshToken",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "TokenId",
                table: "RefreshToken",
                newName: "token_id");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                newName: "IX_RefreshToken_user_id");

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

            migrationBuilder.CreateTable(
                name: "GameGenres",
                columns: table => new
                {
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    GenreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenres", x => new { x.GameId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_GameGenres_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenres_genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "genres",
                        principalColumn: "genre_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatforms",
                columns: table => new
                {
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    PlatformId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatforms", x => new { x.GameId, x.PlatformId });
                    table.ForeignKey(
                        name: "FK_GamePlatforms_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatforms_platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "platforms",
                        principalColumn: "platform_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameSeries",
                columns: table => new
                {
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    SeriesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSeries", x => new { x.GameId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_GameSeries_game_series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "game_series",
                        principalColumn: "series_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameSeries_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameCompany_CompanyId",
                table: "GameCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_GameGenres_GenreId",
                table: "GameGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatforms_PlatformId",
                table: "GamePlatforms",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSeries_SeriesId",
                table: "GameSeries",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_users_user_id",
                table: "RefreshToken",
                column: "user_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_users_user_id",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "GameCompany");

            migrationBuilder.DropTable(
                name: "GameGenres");

            migrationBuilder.DropTable(
                name: "GamePlatforms");

            migrationBuilder.DropTable(
                name: "GameSeries");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "RefreshToken",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "token_id",
                table: "RefreshToken",
                newName: "TokenId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_user_id",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UserId");

            migrationBuilder.CreateTable(
                name: "CompanyDataGameData",
                columns: table => new
                {
                    CompaniesCompanyId = table.Column<long>(type: "bigint", nullable: false),
                    GamesGameId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDataGameData", x => new { x.CompaniesCompanyId, x.GamesGameId });
                    table.ForeignKey(
                        name: "FK_CompanyDataGameData_companies_CompaniesCompanyId",
                        column: x => x.CompaniesCompanyId,
                        principalTable: "companies",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyDataGameData_games_GamesGameId",
                        column: x => x.GamesGameId,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameDataGenreData",
                columns: table => new
                {
                    GamesGameId = table.Column<long>(type: "bigint", nullable: false),
                    GenresGenreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDataGenreData", x => new { x.GamesGameId, x.GenresGenreId });
                    table.ForeignKey(
                        name: "FK_GameDataGenreData_games_GamesGameId",
                        column: x => x.GamesGameId,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDataGenreData_genres_GenresGenreId",
                        column: x => x.GenresGenreId,
                        principalTable: "genres",
                        principalColumn: "genre_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameDataPlatformData",
                columns: table => new
                {
                    GamesGameId = table.Column<long>(type: "bigint", nullable: false),
                    PlatformsPlatformId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDataPlatformData", x => new { x.GamesGameId, x.PlatformsPlatformId });
                    table.ForeignKey(
                        name: "FK_GameDataPlatformData_games_GamesGameId",
                        column: x => x.GamesGameId,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDataPlatformData_platforms_PlatformsPlatformId",
                        column: x => x.PlatformsPlatformId,
                        principalTable: "platforms",
                        principalColumn: "platform_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameDataSeries",
                columns: table => new
                {
                    GamesGameId = table.Column<long>(type: "bigint", nullable: false),
                    SeriesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDataSeries", x => new { x.GamesGameId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_GameDataSeries_game_series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "game_series",
                        principalColumn: "series_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDataSeries_games_GamesGameId",
                        column: x => x.GamesGameId,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDataGameData_GamesGameId",
                table: "CompanyDataGameData",
                column: "GamesGameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDataGenreData_GenresGenreId",
                table: "GameDataGenreData",
                column: "GenresGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDataPlatformData_PlatformsPlatformId",
                table: "GameDataPlatformData",
                column: "PlatformsPlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDataSeries_SeriesId",
                table: "GameDataSeries",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
