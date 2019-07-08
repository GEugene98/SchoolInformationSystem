using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class StudentAchivment_dropFileRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_StudentAchivments_StudentAchivmentId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_StudentAchivmentId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "StudentAchivmentId",
                table: "Files");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentAchivmentId",
                table: "Files",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_StudentAchivmentId",
                table: "Files",
                column: "StudentAchivmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_StudentAchivments_StudentAchivmentId",
                table: "Files",
                column: "StudentAchivmentId",
                principalTable: "StudentAchivments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
