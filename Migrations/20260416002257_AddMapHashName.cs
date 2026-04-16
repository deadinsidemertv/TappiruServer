using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TappiruServer.Migrations
{
    /// <inheritdoc />
    public partial class AddMapHashName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MapHash",
                table: "Scores",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapHash",
                table: "Scores");
        }
    }
}
