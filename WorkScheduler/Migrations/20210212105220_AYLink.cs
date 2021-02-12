using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class AYLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicYearId",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_AcademicYearId",
                table: "Groups",
                column: "AcademicYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AcademicYears_AcademicYearId",
                table: "Groups",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AcademicYears_AcademicYearId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_AcademicYearId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Groups");
        }
    }
}
