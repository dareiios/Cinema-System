using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaSystem.Core.Migrations
{
    public partial class poster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Poster",
                table: "Cinemas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poster",
                table: "Cinemas");
        }
    }
}
