using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Database.Migrations
{
    public partial class Dictionaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dictionary",
                columns: table => new
                {
                    LanguageInName = table.Column<string>(nullable: false),
                    LanguageOutName = table.Column<string>(nullable: false),
                    Index = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dictionary", x => new { x.LanguageInName, x.LanguageOutName });
                    table.UniqueConstraint("AK_Dictionary_Index", x => x.Index);
                    table.ForeignKey(
                        name: "FK_Dictionary_Language_LanguageInName",
                        column: x => x.LanguageInName,
                        principalTable: "Language",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Dictionary_Language_LanguageOutName",
                        column: x => x.LanguageOutName,
                        principalTable: "Language",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DictionaryIndex = table.Column<int>(nullable: false),
                    WordID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Entry_Dictionary_DictionaryIndex",
                        column: x => x.DictionaryIndex,
                        principalTable: "Dictionary",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entry_Word_WordID",
                        column: x => x.WordID,
                        principalTable: "Word",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Meaning",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryID = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meaning", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Meaning_Entry_EntryID",
                        column: x => x.EntryID,
                        principalTable: "Entry",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expression",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: true),
                    Translation = table.Column<string>(nullable: true),
                    EntryID = table.Column<int>(nullable: true),
                    MeaningID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expression", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Expression_Entry_EntryID",
                        column: x => x.EntryID,
                        principalTable: "Entry",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Expression_Meaning_MeaningID",
                        column: x => x.MeaningID,
                        principalTable: "Meaning",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_LanguageOutName",
                table: "Dictionary",
                column: "LanguageOutName");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_DictionaryIndex",
                table: "Entry",
                column: "DictionaryIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_WordID",
                table: "Entry",
                column: "WordID");

            migrationBuilder.CreateIndex(
                name: "IX_Expression_EntryID",
                table: "Expression",
                column: "EntryID");

            migrationBuilder.CreateIndex(
                name: "IX_Expression_MeaningID",
                table: "Expression",
                column: "MeaningID");

            migrationBuilder.CreateIndex(
                name: "IX_Meaning_EntryID",
                table: "Meaning",
                column: "EntryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expression");

            migrationBuilder.DropTable(
                name: "Meaning");

            migrationBuilder.DropTable(
                name: "Entry");

            migrationBuilder.DropTable(
                name: "Dictionary");
        }
    }
}
