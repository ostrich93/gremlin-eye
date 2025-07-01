using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gremlin_eye.Server.Migrations
{
    /// <inheritdoc />
    public partial class PlayLogTimeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "log_time",
                table: "play_logs");

            migrationBuilder.RenameColumn(
                name: "log_date",
                table: "play_logs",
                newName: "start_date");

            migrationBuilder.AddColumn<DateOnly>(
                name: "end_date",
                table: "play_logs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "hours",
                table: "play_logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "minutes",
                table: "play_logs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_date",
                table: "play_logs");

            migrationBuilder.DropColumn(
                name: "hours",
                table: "play_logs");

            migrationBuilder.DropColumn(
                name: "minutes",
                table: "play_logs");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "play_logs",
                newName: "log_date");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "log_time",
                table: "play_logs",
                type: "time",
                nullable: true);
        }
    }
}
