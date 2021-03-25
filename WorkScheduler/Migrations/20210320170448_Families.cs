using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WorkScheduler.Migrations
{
    public partial class Families : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    StudentId = table.Column<int>(nullable: false),
                    PassportNumber = table.Column<string>(nullable: true),
                    BirthCertificate = table.Column<string>(nullable: true),
                    RegistrAddres = table.Column<string>(nullable: true),
                    ResidAddres = table.Column<string>(nullable: true),
                    FullNameMather = table.Column<string>(nullable: true),
                    PhoneMother = table.Column<string>(nullable: true),
                    WorkMother = table.Column<string>(nullable: true),
                    FullNameFather = table.Column<string>(nullable: true),
                    PhoneFather = table.Column<string>(nullable: true),
                    WorkFather = table.Column<string>(nullable: true),
                    FamilyСomposition = table.Column<int>(nullable: false),
                    ClarifyFamilyСomposition = table.Column<int>(nullable: false),
                    FamilyNumberChildren = table.Column<int>(nullable: false),
                    PhysicalGroup = table.Column<int>(nullable: false),
                    Registration = table.Column<int>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Families_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Families_StudentId",
                table: "Families",
                column: "StudentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Families");
        }
    }
}
