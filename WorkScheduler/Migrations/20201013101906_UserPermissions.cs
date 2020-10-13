using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class UserPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanAccept",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanConfirm",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanSeeAllChecklists",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanSeeAllProtocols",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanSeeAllSchedules",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanUseChecklists",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanAccept",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanConfirm",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanSeeAllChecklists",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanSeeAllProtocols",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanSeeAllSchedules",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanUseChecklists",
                table: "AspNetUsers");
        }
    }
}
