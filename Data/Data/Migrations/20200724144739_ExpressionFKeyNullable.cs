using Microsoft.EntityFrameworkCore.Migrations;

namespace Dictionary_MVC.Data.Migrations
{
    public partial class ExpressionFKeyNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expression_Dictionary_DictionaryIndex",
                table: "Expression");

            migrationBuilder.DropForeignKey(
                name: "FK_Expression_Meaning_MeaningID",
                table: "Expression");

            migrationBuilder.AlterColumn<int>(
                name: "MeaningID",
                table: "Expression",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DictionaryIndex",
                table: "Expression",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_Dictionary_DictionaryIndex",
                table: "Expression",
                column: "DictionaryIndex",
                principalTable: "Dictionary",
                principalColumn: "Index",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_Meaning_MeaningID",
                table: "Expression",
                column: "MeaningID",
                principalTable: "Meaning",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expression_Dictionary_DictionaryIndex",
                table: "Expression");

            migrationBuilder.DropForeignKey(
                name: "FK_Expression_Meaning_MeaningID",
                table: "Expression");

            migrationBuilder.AlterColumn<int>(
                name: "MeaningID",
                table: "Expression",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DictionaryIndex",
                table: "Expression",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_Dictionary_DictionaryIndex",
                table: "Expression",
                column: "DictionaryIndex",
                principalTable: "Dictionary",
                principalColumn: "Index",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_Meaning_MeaningID",
                table: "Expression",
                column: "MeaningID",
                principalTable: "Meaning",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
