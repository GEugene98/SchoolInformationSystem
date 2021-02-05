using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WorkScheduler.Migrations
{
    public partial class Contracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    SchoolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    OrganizationId = table.Column<int>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    SigningDate = table.Column<DateTime>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    SignedById = table.Column<string>(nullable: true),
                    Sum = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: true),
                    ControlDate = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    SchoolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_AspNetUsers_SignedById",
                        column: x => x.SignedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_OrganizationId",
                table: "Contracts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SchoolId",
                table: "Contracts",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SignedById",
                table: "Contracts",
                column: "SignedById");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_SchoolId",
                table: "Organizations",
                column: "SchoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
