using Microsoft.EntityFrameworkCore.Migrations;

namespace Dictionary_MVC.Data.Migrations
{
    public partial class SpeechProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "SpeechPart",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    LanguageName = table.Column<string>(nullable: false),
                    Index = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeechPart", x => new { x.LanguageName, x.Name });
                    table.UniqueConstraint("AK_SpeechPart_Index", x => x.Index);
                    table.ForeignKey(
                        name: "FK_SpeechPart_Language_LanguageName",
                        column: x => x.LanguageName,
                        principalTable: "Language",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Word",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceLanguageName = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    SpeechPartName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Word", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Word_Language_SourceLanguageName",
                        column: x => x.SourceLanguageName,
                        principalTable: "Language",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeechPartProperty",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpeechPartIndex = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PossibleValues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeechPartProperty", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SpeechPartProperty_SpeechPart_SpeechPartIndex",
                        column: x => x.SpeechPartIndex,
                        principalTable: "SpeechPart",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordProperty",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    WordID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordProperty", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WordProperty_Word_WordID",
                        column: x => x.WordID,
                        principalTable: "Word",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeechPartProperty_SpeechPartIndex",
                table: "SpeechPartProperty",
                column: "SpeechPartIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Word_SourceLanguageName",
                table: "Word",
                column: "SourceLanguageName");

            migrationBuilder.CreateIndex(
                name: "IX_WordProperty_WordID",
                table: "WordProperty",
                column: "WordID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeechPartProperty");

            migrationBuilder.DropTable(
                name: "WordProperty");

            migrationBuilder.DropTable(
                name: "SpeechPart");

            migrationBuilder.DropTable(
                name: "Word");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}
