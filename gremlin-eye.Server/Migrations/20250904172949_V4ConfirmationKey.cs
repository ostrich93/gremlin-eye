using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gremlin_eye.Server.Migrations
{
    /// <inheritdoc />
    public partial class V4ConfirmationKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "confirmationToken",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "stamp",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmationToken",
                table: "users");

            migrationBuilder.DropColumn(
                name: "stamp",
                table: "users");
        }
    }
}
