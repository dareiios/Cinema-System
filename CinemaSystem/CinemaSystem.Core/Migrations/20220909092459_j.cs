using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaSystem.Core.Migrations
{
    public partial class j : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatTypeId",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SeatTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SeatTypeId",
                table: "Seats",
                column: "SeatTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_SeatTypes_SeatTypeId",
                table: "Seats",
                column: "SeatTypeId",
                principalTable: "SeatTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_SeatTypes_SeatTypeId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "SeatTypes");

            migrationBuilder.DropIndex(
                name: "IX_Seats_SeatTypeId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatTypeId",
                table: "Seats");
        }
    }
}
