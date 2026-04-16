using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TappiruServer.Migrations
{
    /// <inheritdoc />
    public partial class AddTpToScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TP",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TP",
                table: "Scores");
        }
    }
}
