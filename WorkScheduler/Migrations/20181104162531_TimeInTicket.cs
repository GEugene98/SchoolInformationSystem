using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class TimeInTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Hours",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Minutes",
                table: "Tickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "Tickets");
        }
    }
}
