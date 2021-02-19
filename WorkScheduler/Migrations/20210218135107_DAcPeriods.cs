using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WorkScheduler.Migrations
{
    public partial class DAcPeriods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaningRecords_AcademicPeriods_AcademicPeriodId",
                table: "PlaningRecords");

            migrationBuilder.DropTable(
                name: "AcademicPeriods");

            migrationBuilder.DropIndex(
                name: "IX_PlaningRecords_AcademicPeriodId",
                table: "PlaningRecords");

            migrationBuilder.DropColumn(
                name: "AcademicPeriodId",
                table: "PlaningRecords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicPeriodId",
                table: "PlaningRecords",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AcademicPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    End = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SchoolId = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicPeriods_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaningRecords_AcademicPeriodId",
                table: "PlaningRecords",
                column: "AcademicPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPeriods_SchoolId",
                table: "AcademicPeriods",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaningRecords_AcademicPeriods_AcademicPeriodId",
                table: "PlaningRecords",
                column: "AcademicPeriodId",
                principalTable: "AcademicPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
