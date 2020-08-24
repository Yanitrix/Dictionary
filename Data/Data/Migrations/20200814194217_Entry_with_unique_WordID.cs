using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Database.Migrations
{
    public partial class Entry_with_unique_WordID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Entry_WordID",
                table: "Entry");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_WordID",
                table: "Entry",
                column: "WordID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Entry_WordID",
                table: "Entry");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_WordID",
                table: "Entry",
                column: "WordID");
        }
    }
}
