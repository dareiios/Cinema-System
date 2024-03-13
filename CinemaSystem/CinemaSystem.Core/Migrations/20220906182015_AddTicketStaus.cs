using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaSystem.Core.Migrations
{
    public partial class AddTicketStaus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Seats");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
