using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class SchoolDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionNamesToMakeProtocolJSON",
                table: "Schools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentHeaderHTML",
                table: "Schools",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionNamesToMakeProtocolJSON",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "DocumentHeaderHTML",
                table: "Schools");
        }
    }
}
