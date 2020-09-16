using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Database.Migrations
{
    public partial class RemoveSpeechPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeechPartProperty");

            migrationBuilder.DropTable(
                name: "SpeechPart");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "WordProperty");

            migrationBuilder.DropColumn(
                name: "SpeechPartName",
                table: "Word");

            migrationBuilder.AddColumn<string>(
                name: "Values",
                table: "WordProperty",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Values",
                table: "WordProperty");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "WordProperty",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpeechPartName",
                table: "Word",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SpeechPart",
                columns: table => new
                {
                    LanguageName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false)
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
                name: "SpeechPartProperty",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PossibleValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpeechPartIndex = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_SpeechPartProperty_SpeechPartIndex",
                table: "SpeechPartProperty",
                column: "SpeechPartIndex");
        }
    }
}
