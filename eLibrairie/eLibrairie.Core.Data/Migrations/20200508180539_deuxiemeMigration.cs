using Microsoft.EntityFrameworkCore.Migrations;

namespace eLibrairie.Core.Data.Migrations
{
    public partial class deuxiemeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Book_CategorieId",
                table: "Book",
                column: "CategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Categorie_CategorieId",
                table: "Book",
                column: "CategorieId",
                principalTable: "Categorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Categorie_CategorieId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_CategorieId",
                table: "Book");
        }
    }
}
