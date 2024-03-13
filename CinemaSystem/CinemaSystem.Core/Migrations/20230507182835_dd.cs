using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaSystem.Core.Migrations
{
    public partial class dd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PromoDuration",
                table: "Seances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromoDuration",
                table: "Seances");
        }
    }
}
