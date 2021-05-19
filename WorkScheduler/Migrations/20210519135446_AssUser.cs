using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class AssUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Associations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Associations_UserId",
                table: "Associations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Associations_AspNetUsers_UserId",
                table: "Associations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Associations_AspNetUsers_UserId",
                table: "Associations");

            migrationBuilder.DropIndex(
                name: "IX_Associations_UserId",
                table: "Associations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Associations");
        }
    }
}
