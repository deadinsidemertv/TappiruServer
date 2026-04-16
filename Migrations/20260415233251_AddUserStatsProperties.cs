using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TappiruServer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStatsProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Accuracy",
                table: "AspNetUsers",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "MaxCombo",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "TotalPlayTime",
                table: "AspNetUsers",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accuracy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxCombo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalPlayTime",
                table: "AspNetUsers");
        }
    }
}
