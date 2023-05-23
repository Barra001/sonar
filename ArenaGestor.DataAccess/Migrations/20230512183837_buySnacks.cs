using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArenaGestor.DataAccess.Migrations
{
    public partial class buySnacks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SnackBuy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SnackId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnackBuy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SnackBuy_Snack_SnackId",
                        column: x => x.SnackId,
                        principalTable: "Snack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnackBuy_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SnackBuy_SnackId",
                table: "SnackBuy",
                column: "SnackId");

            migrationBuilder.CreateIndex(
                name: "IX_SnackBuy_TicketId",
                table: "SnackBuy",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SnackBuy");
        }
    }
}
