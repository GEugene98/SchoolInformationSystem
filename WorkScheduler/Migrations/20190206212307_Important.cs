using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class Important : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Tickets");

            migrationBuilder.AddColumn<bool>(
                name: "Important",
                table: "Tickets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Important",
                table: "Tickets");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "End",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start",
                table: "Tickets",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
