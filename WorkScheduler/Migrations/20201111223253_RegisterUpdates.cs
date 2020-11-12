using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WorkScheduler.Migrations
{
    public partial class RegisterUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAchivments_AchivmentLevels_AchivmentLevelId",
                table: "StudentAchivments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAchivments_AchivmentResults_AchivmentResultId",
                table: "StudentAchivments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAchivments_StudentActions_StudentActionId",
                table: "StudentAchivments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentActions_Actions_ActionId",
                table: "StudentActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentActions",
                table: "StudentActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAchivments",
                table: "StudentAchivments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AchivmentResults",
                table: "AchivmentResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AchivmentLevels",
                table: "AchivmentLevels");

            migrationBuilder.RenameTable(
                name: "StudentActions",
                newName: "StudentAction");

            migrationBuilder.RenameTable(
                name: "StudentAchivments",
                newName: "StudentAchivment");

            migrationBuilder.RenameTable(
                name: "AchivmentResults",
                newName: "AchivmentResult");

            migrationBuilder.RenameTable(
                name: "AchivmentLevels",
                newName: "AchivmentLevel");

            migrationBuilder.RenameIndex(
                name: "IX_StudentActions_ActionId",
                table: "StudentAction",
                newName: "IX_StudentAction_ActionId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAchivments_StudentActionId",
                table: "StudentAchivment",
                newName: "IX_StudentAchivment_StudentActionId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAchivments_AchivmentResultId",
                table: "StudentAchivment",
                newName: "IX_StudentAchivment_AchivmentResultId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAchivments_AchivmentLevelId",
                table: "StudentAchivment",
                newName: "IX_StudentAchivment_AchivmentLevelId");

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Students",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Groups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Classes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAction",
                table: "StudentAction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAchivment",
                table: "StudentAchivment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AchivmentResult",
                table: "AchivmentResult",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AchivmentLevel",
                table: "AchivmentLevel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AcademicPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    SchoolId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Associations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    AcademicYearId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SchoolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Associations_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Associations_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssociationGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false),
                    AssociationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociationGroups", x => new { x.GroupId, x.AssociationId });
                    table.ForeignKey(
                        name: "FK_AssociationGroups_Associations_AssociationId",
                        column: x => x.AssociationId,
                        principalTable: "Associations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssociationGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaningRecords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Hours = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    AssociationId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    AcademicYearId = table.Column<int>(nullable: false),
                    AcademicPeriodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaningRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaningRecords_AcademicPeriods_AcademicPeriodId",
                        column: x => x.AcademicPeriodId,
                        principalTable: "AcademicPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaningRecords_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaningRecords_Associations_AssociationId",
                        column: x => x.AssociationId,
                        principalTable: "Associations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaningRecords_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegisterRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Content = table.Column<string>(nullable: true),
                    PlaningRecordId = table.Column<long>(nullable: false),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisterRecords_PlaningRecords_PlaningRecordId",
                        column: x => x.PlaningRecordId,
                        principalTable: "PlaningRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegisterRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SchoolId",
                table: "Groups",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolId",
                table: "Classes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPeriods_SchoolId",
                table: "AcademicPeriods",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AssociationGroups_AssociationId",
                table: "AssociationGroups",
                column: "AssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_Associations_AcademicYearId",
                table: "Associations",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Associations_SchoolId",
                table: "Associations",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaningRecords_AcademicPeriodId",
                table: "PlaningRecords",
                column: "AcademicPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaningRecords_AcademicYearId",
                table: "PlaningRecords",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaningRecords_AssociationId",
                table: "PlaningRecords",
                column: "AssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaningRecords_GroupId",
                table: "PlaningRecords",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterRecords_PlaningRecordId",
                table: "RegisterRecords",
                column: "PlaningRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterRecords_StudentId",
                table: "RegisterRecords",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Schools_SchoolId",
                table: "Classes",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Schools_SchoolId",
                table: "Groups",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAchivment_AchivmentLevel_AchivmentLevelId",
                table: "StudentAchivment",
                column: "AchivmentLevelId",
                principalTable: "AchivmentLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAchivment_AchivmentResult_AchivmentResultId",
                table: "StudentAchivment",
                column: "AchivmentResultId",
                principalTable: "AchivmentResult",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAchivment_StudentAction_StudentActionId",
                table: "StudentAchivment",
                column: "StudentActionId",
                principalTable: "StudentAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAction_Actions_ActionId",
                table: "StudentAction",
                column: "ActionId",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Schools_SchoolId",
                table: "Students",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Schools_SchoolId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Schools_SchoolId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAchivment_AchivmentLevel_AchivmentLevelId",
                table: "StudentAchivment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAchivment_AchivmentResult_AchivmentResultId",
                table: "StudentAchivment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAchivment_StudentAction_StudentActionId",
                table: "StudentAchivment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAction_Actions_ActionId",
                table: "StudentAction");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Schools_SchoolId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "AssociationGroups");

            migrationBuilder.DropTable(
                name: "RegisterRecords");

            migrationBuilder.DropTable(
                name: "PlaningRecords");

            migrationBuilder.DropTable(
                name: "AcademicPeriods");

            migrationBuilder.DropTable(
                name: "Associations");

            migrationBuilder.DropIndex(
                name: "IX_Students_SchoolId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Groups_SchoolId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Classes_SchoolId",
                table: "Classes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAction",
                table: "StudentAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAchivment",
                table: "StudentAchivment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AchivmentResult",
                table: "AchivmentResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AchivmentLevel",
                table: "AchivmentLevel");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Classes");

            migrationBuilder.RenameTable(
                name: "StudentAction",
                newName: "StudentActions");

            migrationBuilder.RenameTable(
                name: "StudentAchivment",
                newName: "StudentAchivments");

            migrationBuilder.RenameTable(
                name: "AchivmentResult",
                newName: "AchivmentResults");

            migrationBuilder.RenameTable(
                name: "AchivmentLevel",
                newName: "AchivmentLevels");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAction_ActionId",
                table: "StudentActions",
                newName: "IX_StudentActions_ActionId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAchivment_StudentActionId",
                table: "StudentAchivments",
                newName: "IX_StudentAchivments_StudentActionId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAchivment_AchivmentResultId",
                table: "StudentAchivments",
                newName: "IX_StudentAchivments_AchivmentResultId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAchivment_AchivmentLevelId",
                table: "StudentAchivments",
                newName: "IX_StudentAchivments_AchivmentLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentActions",
                table: "StudentActions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAchivments",
                table: "StudentAchivments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AchivmentResults",
                table: "AchivmentResults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AchivmentLevels",
                table: "AchivmentLevels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAchivments_AchivmentLevels_AchivmentLevelId",
                table: "StudentAchivments",
                column: "AchivmentLevelId",
                principalTable: "AchivmentLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAchivments_AchivmentResults_AchivmentResultId",
                table: "StudentAchivments",
                column: "AchivmentResultId",
                principalTable: "AchivmentResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAchivments_StudentActions_StudentActionId",
                table: "StudentAchivments",
                column: "StudentActionId",
                principalTable: "StudentActions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentActions_Actions_ActionId",
                table: "StudentActions",
                column: "ActionId",
                principalTable: "Actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
