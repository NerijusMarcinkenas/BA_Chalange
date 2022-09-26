using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksSpot.Data.Migrations
{
    public partial class BookRezervationInfoTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookInfoId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookRezervations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RezerverdDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RezervationExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRezervations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookInfoId",
                table: "Books",
                column: "BookInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookRezervations_BookInfoId",
                table: "Books",
                column: "BookInfoId",
                principalTable: "BookRezervations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookRezervations_BookInfoId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookRezervations");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookInfoId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookInfoId",
                table: "Books");
        }
    }
}
