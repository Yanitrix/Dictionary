using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Database.Migrations
{
    public partial class WordProperty_Word_NavProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordProperty_Word_WordID",
                table: "WordProperty");

            migrationBuilder.AlterColumn<int>(
                name: "WordID",
                table: "WordProperty",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WordProperty_Word_WordID",
                table: "WordProperty",
                column: "WordID",
                principalTable: "Word",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordProperty_Word_WordID",
                table: "WordProperty");

            migrationBuilder.AlterColumn<int>(
                name: "WordID",
                table: "WordProperty",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_WordProperty_Word_WordID",
                table: "WordProperty",
                column: "WordID",
                principalTable: "Word",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
