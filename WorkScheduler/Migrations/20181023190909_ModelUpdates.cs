using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WorkScheduler.Migrations
{
    public partial class ModelUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actions_Status_StatusId",
                table: "Actions");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Actions_StatusId",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "NeedConfirm",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "IsFreezed",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Actions");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Actions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Actions");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "WorkSchedules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "WorkSchedules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedConfirm",
                table: "WorkSchedules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFreezed",
                table: "Actions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Actions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actions_StatusId",
                table: "Actions",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_Status_StatusId",
                table: "Actions",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
