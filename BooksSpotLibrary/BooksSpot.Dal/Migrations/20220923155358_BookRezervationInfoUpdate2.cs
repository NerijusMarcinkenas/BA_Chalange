using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksSpot.Data.Migrations
{
    public partial class BookRezervationInfoUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookRezervations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRezervations_UserId",
                table: "BookRezervations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRezervations_Users_UserId",
                table: "BookRezervations",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRezervations_Users_UserId",
                table: "BookRezervations");

            migrationBuilder.DropIndex(
                name: "IX_BookRezervations_UserId",
                table: "BookRezervations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookRezervations");
        }
    }
}
