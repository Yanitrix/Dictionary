using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Database.Migrations
{
    public partial class SplitExpression : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expression");

            migrationBuilder.CreateTable(
                name: "Example",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: false),
                    Translation = table.Column<string>(nullable: false),
                    MeaningID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Example", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Example_Meaning_MeaningID",
                        column: x => x.MeaningID,
                        principalTable: "Meaning",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FreeExpression",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: false),
                    Translation = table.Column<string>(nullable: false),
                    DictionaryIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeExpression", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FreeExpression_Dictionary_DictionaryIndex",
                        column: x => x.DictionaryIndex,
                        principalTable: "Dictionary",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Example_MeaningID",
                table: "Example",
                column: "MeaningID");

            migrationBuilder.CreateIndex(
                name: "IX_FreeExpression_DictionaryIndex",
                table: "FreeExpression",
                column: "DictionaryIndex");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Example");

            migrationBuilder.DropTable(
                name: "FreeExpression");

            migrationBuilder.CreateTable(
                name: "Expression",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DictionaryIndex = table.Column<int>(type: "int", nullable: true),
                    MeaningID = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expression", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Expression_Dictionary_DictionaryIndex",
                        column: x => x.DictionaryIndex,
                        principalTable: "Dictionary",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expression_Meaning_MeaningID",
                        column: x => x.MeaningID,
                        principalTable: "Meaning",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expression_DictionaryIndex",
                table: "Expression",
                column: "DictionaryIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Expression_MeaningID",
                table: "Expression",
                column: "MeaningID");
        }
    }
}
