using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkScheduler.Migrations
{
    public partial class WorkflowImps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomingDocuments_Tickets_TicketId",
                table: "IncomingDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_OutgoingDocuments_Tickets_TicketId",
                table: "OutgoingDocuments");

            migrationBuilder.DropIndex(
                name: "IX_OutgoingDocuments_TicketId",
                table: "OutgoingDocuments");

            migrationBuilder.DropIndex(
                name: "IX_IncomingDocuments_TicketId",
                table: "IncomingDocuments");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "OutgoingDocuments");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "IncomingDocuments");

            migrationBuilder.AddColumn<int>(
                name: "IncomingDocumentId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OnCheck",
                table: "Tickets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OutgoingDocumentId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IncomingDocumentId",
                table: "Tickets",
                column: "IncomingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_OutgoingDocumentId",
                table: "Tickets",
                column: "OutgoingDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_IncomingDocuments_IncomingDocumentId",
                table: "Tickets",
                column: "IncomingDocumentId",
                principalTable: "IncomingDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_OutgoingDocuments_OutgoingDocumentId",
                table: "Tickets",
                column: "OutgoingDocumentId",
                principalTable: "OutgoingDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_IncomingDocuments_IncomingDocumentId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_OutgoingDocuments_OutgoingDocumentId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_IncomingDocumentId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_OutgoingDocumentId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IncomingDocumentId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "OnCheck",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "OutgoingDocumentId",
                table: "Tickets");

            migrationBuilder.AddColumn<long>(
                name: "TicketId",
                table: "OutgoingDocuments",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TicketId",
                table: "IncomingDocuments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingDocuments_TicketId",
                table: "OutgoingDocuments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingDocuments_TicketId",
                table: "IncomingDocuments",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomingDocuments_Tickets_TicketId",
                table: "IncomingDocuments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoingDocuments_Tickets_TicketId",
                table: "OutgoingDocuments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
