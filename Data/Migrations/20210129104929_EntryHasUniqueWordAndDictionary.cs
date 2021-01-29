using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class EntryHasUniqueWordAndDictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Entry_WordID_DictionaryIndex",
                table: "Entry",
                columns: new[] { "WordID", "DictionaryIndex" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Entry_WordID_DictionaryIndex",
                table: "Entry");
        }
    }
}
