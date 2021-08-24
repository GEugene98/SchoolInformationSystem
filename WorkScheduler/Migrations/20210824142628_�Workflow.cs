using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WorkScheduler.Migrations
{
    public partial class Workflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IncomingDocumentTypesJSON",
                table: "Schools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomingWorkflowChecklist",
                table: "Schools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutgoingDocumentTypesJSON",
                table: "Schools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutgoingWorkflowChecklist",
                table: "Schools",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IncomingDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    SchoolId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    TicketId = table.Column<long>(nullable: true),
                    Num = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Taken = table.Column<DateTime>(nullable: true),
                    Deadline = table.Column<DateTime>(nullable: true),
                    Done = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingDocuments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomingDocuments_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomingDocuments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomingDocuments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    SchoolId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    TicketId = table.Column<long>(nullable: true),
                    Num = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Taken = table.Column<DateTime>(nullable: true),
                    Deadline = table.Column<DateTime>(nullable: true),
                    Done = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutgoingDocuments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomingDocumentFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IncomingDocumentId = table.Column<int>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingDocumentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingDocumentFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomingDocumentFiles_IncomingDocuments_IncomingDocumentId",
                        column: x => x.IncomingDocumentId,
                        principalTable: "IncomingDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingDocumentFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    OutgoingDocumentId = table.Column<int>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingDocumentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingDocumentFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutgoingDocumentFiles_OutgoingDocuments_OutgoingDocumentId",
                        column: x => x.OutgoingDocumentId,
                        principalTable: "OutgoingDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocumentFiles_FileId",
                table: "IncomingDocumentFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocumentFiles_IncomingDocumentId",
                table: "IncomingDocumentFiles",
                column: "IncomingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocuments_OrganizationId",
                table: "IncomingDocuments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocuments_SchoolId",
                table: "IncomingDocuments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocuments_TicketId",
                table: "IncomingDocuments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocuments_UserId",
                table: "IncomingDocuments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocumentFiles_FileId",
                table: "OutgoingDocumentFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocumentFiles_OutgoingDocumentId",
                table: "OutgoingDocumentFiles",
                column: "OutgoingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_OrganizationId",
                table: "OutgoingDocuments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_SchoolId",
                table: "OutgoingDocuments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_TicketId",
                table: "OutgoingDocuments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_UserId",
                table: "OutgoingDocuments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomingDocumentFiles");

            migrationBuilder.DropTable(
                name: "OutgoingDocumentFiles");

            migrationBuilder.DropTable(
                name: "IncomingDocuments");

            migrationBuilder.DropTable(
                name: "OutgoingDocuments");

            migrationBuilder.DropColumn(
                name: "IncomingDocumentTypesJSON",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "IncomingWorkflowChecklist",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "OutgoingDocumentTypesJSON",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "OutgoingWorkflowChecklist",
                table: "Schools");
        }
    }
}
