using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Database.Migrations
{
    public partial class ExpressionsOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expression_Entry_EntryID",
                table: "Expression");

            migrationBuilder.DropForeignKey(
                name: "FK_Expression_Meaning_MeaningID",
                table: "Expression");

            migrationBuilder.DropIndex(
                name: "IX_Expression_EntryID",
                table: "Expression");

            migrationBuilder.DropColumn(
                name: "EntryID",
                table: "Expression");

            migrationBuilder.AlterColumn<int>(
                name: "MeaningID",
                table: "Expression",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DictionaryIndex",
                table: "Expression",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Expression_DictionaryIndex",
                table: "Expression",
                column: "DictionaryIndex");

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

            migrationBuilder.DropIndex(
                name: "IX_Expression_DictionaryIndex",
                table: "Expression");

            migrationBuilder.DropColumn(
                name: "DictionaryIndex",
                table: "Expression");

            migrationBuilder.AlterColumn<int>(
                name: "MeaningID",
                table: "Expression",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "EntryID",
                table: "Expression",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expression_EntryID",
                table: "Expression",
                column: "EntryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_Entry_EntryID",
                table: "Expression",
                column: "EntryID",
                principalTable: "Entry",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_Meaning_MeaningID",
                table: "Expression",
                column: "MeaningID",
                principalTable: "Meaning",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
