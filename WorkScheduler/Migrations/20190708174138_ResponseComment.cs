using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class ResponseComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponseComment",
                table: "Tickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseComment",
                table: "Tickets");
        }
    }
}
