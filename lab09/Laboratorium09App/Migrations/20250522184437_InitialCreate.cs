using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laboratorium09App.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dane",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tekst = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dane", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loginy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    Haslo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loginy", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Dane",
                columns: new[] { "Id", "Tekst" },
                values: new object[] { 1, "Przykładowy wpis" });

            migrationBuilder.InsertData(
                table: "Loginy",
                columns: new[] { "Id", "Haslo", "Nazwa" },
                values: new object[] { 1, "81DC9BDB52D04DC20036DBD8313ED055", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dane");

            migrationBuilder.DropTable(
                name: "Loginy");
        }
    }
}
