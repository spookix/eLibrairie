using Microsoft.EntityFrameworkCore.Migrations;

namespace eLibrairie.Core.Data.Migrations
{
    public partial class quatriemeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompteId",
                table: "Commande");

            migrationBuilder.AddColumn<string>(
                name: "CompteMail",
                table: "Commande",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompteMail",
                table: "Commande");

            migrationBuilder.AddColumn<int>(
                name: "CompteId",
                table: "Commande",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
