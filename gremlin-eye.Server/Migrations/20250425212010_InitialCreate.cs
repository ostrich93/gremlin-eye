using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gremlin_eye.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    company_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.company_id);
                });

            migrationBuilder.CreateTable(
                name: "game_series",
                columns: table => new
                {
                    series_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_series", x => x.series_id);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    game_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cover_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    banner_uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    game_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    game_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    release_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    parent_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.game_id);
                    table.ForeignKey(
                        name: "FK_games_games_parent_id",
                        column: x => x.parent_id,
                        principalTable: "games",
                        principalColumn: "game_id");
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    genre_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "platforms",
                columns: table => new
                {
                    platform_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platforms", x => x.platform_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    avatar_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

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
                name: "game_likes",
                columns: table => new
                {
                    like_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    game_slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_likes", x => x.like_id);
                    table.ForeignKey(
                        name: "FK_game_likes_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_likes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_logs",
                columns: table => new
                {
                    game_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    play_status = table.Column<int>(type: "int", nullable: true),
                    is_played = table.Column<bool>(type: "bit", nullable: false),
                    is_playing = table.Column<bool>(type: "bit", nullable: false),
                    is_backlog = table.Column<bool>(type: "bit", nullable: false),
                    is_wishlist = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_logs", x => x.game_log_id);
                    table.ForeignKey(
                        name: "FK_game_logs_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "game_id");
                    table.ForeignKey(
                        name: "FK_game_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listings",
                columns: table => new
                {
                    listing_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_public = table.Column<bool>(type: "bit", nullable: false),
                    is_ranked = table.Column<bool>(type: "bit", nullable: false),
                    is_grid = table.Column<bool>(type: "bit", nullable: false),
                    is_desc = table.Column<bool>(type: "bit", nullable: false),
                    comments_locked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listings", x => x.listing_id);
                    table.ForeignKey(
                        name: "FK_listings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    TokenId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_RefreshToken_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playthroughs",
                columns: table => new
                {
                    playthrough_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    game_log_id = table.Column<long>(type: "bigint", nullable: false),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    log_title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    time_played = table.Column<TimeSpan>(type: "time", nullable: true),
                    is_mastered = table.Column<bool>(type: "bit", nullable: false),
                    is_replay = table.Column<bool>(type: "bit", nullable: false),
                    medium = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    played_on = table.Column<long>(type: "bigint", nullable: true),
                    review = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    review_spoilers = table.Column<bool>(type: "bit", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playthroughs", x => x.playthrough_id);
                    table.ForeignKey(
                        name: "FK_playthroughs_game_logs_game_log_id",
                        column: x => x.game_log_id,
                        principalTable: "game_logs",
                        principalColumn: "game_log_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playthroughs_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playthroughs_platforms_played_on",
                        column: x => x.played_on,
                        principalTable: "platforms",
                        principalColumn: "platform_id");
                });

            migrationBuilder.CreateTable(
                name: "ListEntries",
                columns: table => new
                {
                    entry_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    entry_note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListEntries", x => x.entry_id);
                    table.ForeignKey(
                        name: "FK_ListEntries_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListEntries_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "listing_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_comments",
                columns: table => new
                {
                    comment_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    comment_body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_listing_comments_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "listing_id");
                    table.ForeignKey(
                        name: "FK_listing_comments_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_likes",
                columns: table => new
                {
                    like_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_likes", x => x.like_id);
                    table.ForeignKey(
                        name: "FK_listing_likes_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "listing_id");
                    table.ForeignKey(
                        name: "FK_listing_likes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "play_logs",
                columns: table => new
                {
                    play_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    playthrough_id = table.Column<long>(type: "bigint", nullable: false),
                    log_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    log_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_start = table.Column<bool>(type: "bit", nullable: false),
                    is_end = table.Column<bool>(type: "bit", nullable: false),
                    log_note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_play_logs", x => x.play_log_id);
                    table.ForeignKey(
                        name: "FK_play_logs_playthroughs_playthrough_id",
                        column: x => x.playthrough_id,
                        principalTable: "playthroughs",
                        principalColumn: "playthrough_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    review_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    game_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    playthrough_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    comments_locked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_reviews_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviews_playthroughs_playthrough_id",
                        column: x => x.playthrough_id,
                        principalTable: "playthroughs",
                        principalColumn: "playthrough_id");
                    table.ForeignKey(
                        name: "FK_reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "review_comments",
                columns: table => new
                {
                    comment_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    review_id = table.Column<long>(type: "bigint", nullable: false),
                    comment_body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_review_comments_reviews_review_id",
                        column: x => x.review_id,
                        principalTable: "reviews",
                        principalColumn: "review_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_comments_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review_likes",
                columns: table => new
                {
                    like_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    review_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_likes", x => x.like_id);
                    table.ForeignKey(
                        name: "FK_review_likes_reviews_review_id",
                        column: x => x.review_id,
                        principalTable: "reviews",
                        principalColumn: "review_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_likes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDataGameData_GamesGameId",
                table: "CompanyDataGameData",
                column: "GamesGameId");

            migrationBuilder.CreateIndex(
                name: "IX_game_likes_game_id",
                table: "game_likes",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_likes_user_id",
                table: "game_likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_logs_game_id",
                table: "game_logs",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_logs_user_id",
                table: "game_logs",
                column: "user_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_games_parent_id",
                table: "games",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_ListEntries_game_id",
                table: "ListEntries",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_ListEntries_listing_id",
                table: "ListEntries",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_listing_comments_author_id",
                table: "listing_comments",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_listing_comments_listing_id",
                table: "listing_comments",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_listing_likes_listing_id",
                table: "listing_likes",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_listing_likes_user_id",
                table: "listing_likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_listings_user_id",
                table: "listings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_play_logs_playthrough_id",
                table: "play_logs",
                column: "playthrough_id");

            migrationBuilder.CreateIndex(
                name: "IX_playthroughs_game_id",
                table: "playthroughs",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_playthroughs_game_log_id",
                table: "playthroughs",
                column: "game_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_playthroughs_played_on",
                table: "playthroughs",
                column: "played_on");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_review_comments_author_id",
                table: "review_comments",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_comments_review_id",
                table: "review_comments",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_likes_review_id",
                table: "review_likes",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_likes_user_id",
                table: "review_likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_game_id",
                table: "reviews",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_playthrough_id",
                table: "reviews",
                column: "playthrough_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reviews_user_id",
                table: "reviews",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyDataGameData");

            migrationBuilder.DropTable(
                name: "game_likes");

            migrationBuilder.DropTable(
                name: "GameDataGenreData");

            migrationBuilder.DropTable(
                name: "GameDataPlatformData");

            migrationBuilder.DropTable(
                name: "GameDataSeries");

            migrationBuilder.DropTable(
                name: "ListEntries");

            migrationBuilder.DropTable(
                name: "listing_comments");

            migrationBuilder.DropTable(
                name: "listing_likes");

            migrationBuilder.DropTable(
                name: "play_logs");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "review_comments");

            migrationBuilder.DropTable(
                name: "review_likes");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "game_series");

            migrationBuilder.DropTable(
                name: "listings");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "playthroughs");

            migrationBuilder.DropTable(
                name: "game_logs");

            migrationBuilder.DropTable(
                name: "platforms");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
