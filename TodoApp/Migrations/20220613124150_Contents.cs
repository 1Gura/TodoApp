using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Migrations
{
    public partial class Contents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentBase_PageNotes_PageNoteId",
                table: "ContentBase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentBase",
                table: "ContentBase");

            migrationBuilder.RenameTable(
                name: "ContentBase",
                newName: "Contents");

            migrationBuilder.RenameIndex(
                name: "IX_ContentBase_PageNoteId",
                table: "Contents",
                newName: "IX_Contents_PageNoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contents",
                table: "Contents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_PageNotes_PageNoteId",
                table: "Contents",
                column: "PageNoteId",
                principalTable: "PageNotes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_PageNotes_PageNoteId",
                table: "Contents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contents",
                table: "Contents");

            migrationBuilder.RenameTable(
                name: "Contents",
                newName: "ContentBase");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_PageNoteId",
                table: "ContentBase",
                newName: "IX_ContentBase_PageNoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentBase",
                table: "ContentBase",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentBase_PageNotes_PageNoteId",
                table: "ContentBase",
                column: "PageNoteId",
                principalTable: "PageNotes",
                principalColumn: "Id");
        }
    }
}
