﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Everything : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false, collation: "SQL_Latin1_General_CP1_CS_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Dictionary",
                columns: table => new
                {
                    LanguageInName = table.Column<string>(type: "nvarchar(450)", nullable: false, collation: "SQL_Latin1_General_CP1_CS_AS"),
                    LanguageOutName = table.Column<string>(type: "nvarchar(450)", nullable: false, collation: "SQL_Latin1_General_CP1_CS_AS"),
                    Index = table.Column<int>(type: "int", nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dictionary_Language_LanguageOutName",
                        column: x => x.LanguageOutName,
                        principalTable: "Language",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Word",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceLanguageName = table.Column<string>(type: "nvarchar(450)", nullable: false, collation: "SQL_Latin1_General_CP1_CS_AS"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_Latin1_General_CP1_CS_AS")
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
                name: "FreeExpression",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DictionaryIndex = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DictionaryIndex = table.Column<int>(type: "int", nullable: false),
                    WordID = table.Column<int>(type: "int", nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordProperty",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Values = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordProperty", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WordProperty_Word_WordID",
                        column: x => x.WordID,
                        principalTable: "Word",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meaning",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Example",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeaningID = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_LanguageOutName",
                table: "Dictionary",
                column: "LanguageOutName");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_DictionaryIndex",
                table: "Entry",
                column: "DictionaryIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Entry_WordID_DictionaryIndex",
                table: "Entry",
                columns: new[] { "WordID", "DictionaryIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Example_MeaningID",
                table: "Example",
                column: "MeaningID");

            migrationBuilder.CreateIndex(
                name: "IX_FreeExpression_DictionaryIndex",
                table: "FreeExpression",
                column: "DictionaryIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Meaning_EntryID",
                table: "Meaning",
                column: "EntryID");

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
                name: "Example");

            migrationBuilder.DropTable(
                name: "FreeExpression");

            migrationBuilder.DropTable(
                name: "WordProperty");

            migrationBuilder.DropTable(
                name: "Meaning");

            migrationBuilder.DropTable(
                name: "Entry");

            migrationBuilder.DropTable(
                name: "Dictionary");

            migrationBuilder.DropTable(
                name: "Word");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}
