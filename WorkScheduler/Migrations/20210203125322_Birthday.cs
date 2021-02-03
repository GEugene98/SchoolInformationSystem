using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class Birthday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "Students",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Students");
        }
    }
}
