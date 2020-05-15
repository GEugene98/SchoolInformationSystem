using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class ProtocolChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agenda",
                table: "Protocols");

            migrationBuilder.DropColumn(
                name: "Decided",
                table: "Protocols");

            migrationBuilder.DropColumn(
                name: "Listen",
                table: "Protocols");

            migrationBuilder.RenameColumn(
                name: "Speaked",
                table: "Protocols",
                newName: "ProtocolContentJSON");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProtocolContentJSON",
                table: "Protocols",
                newName: "Speaked");

            migrationBuilder.AddColumn<string>(
                name: "Agenda",
                table: "Protocols",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Decided",
                table: "Protocols",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Listen",
                table: "Protocols",
                nullable: true);
        }
    }
}
