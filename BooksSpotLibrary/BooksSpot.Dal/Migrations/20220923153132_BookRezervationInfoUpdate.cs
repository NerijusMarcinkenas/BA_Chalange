using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksSpot.Data.Migrations
{
    public partial class BookRezervationInfoUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookRezervations_BookInfoId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookInfoId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookInfoId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "BookRezervations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRezervations_BookId",
                table: "BookRezervations",
                column: "BookId",
                unique: true,
                filter: "[BookId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRezervations_Books_BookId",
                table: "BookRezervations",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRezervations_Books_BookId",
                table: "BookRezervations");

            migrationBuilder.DropIndex(
                name: "IX_BookRezervations_BookId",
                table: "BookRezervations");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BookRezervations");

            migrationBuilder.AddColumn<int>(
                name: "BookInfoId",
                table: "Books",
                type: "int",
                nullable: true);

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
    }
}
