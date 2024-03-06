using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class colabmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collab_Note_NotesEntityNoteId",
                table: "Collab");

            migrationBuilder.DropForeignKey(
                name: "FK_Collab_User_NotesUserId",
                table: "Collab");

            migrationBuilder.DropIndex(
                name: "IX_Collab_NotesEntityNoteId",
                table: "Collab");

            migrationBuilder.DropIndex(
                name: "IX_Collab_NotesUserId",
                table: "Collab");

            migrationBuilder.DropColumn(
                name: "NotesEntityNoteId",
                table: "Collab");

            migrationBuilder.DropColumn(
                name: "NotesUserId",
                table: "Collab");

            migrationBuilder.CreateIndex(
                name: "IX_Collab_Id",
                table: "Collab",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Collab_NoteId",
                table: "Collab",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collab_User_Id",
                table: "Collab",
                column: "Id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Collab_Note_NoteId",
                table: "Collab",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collab_User_Id",
                table: "Collab");

            migrationBuilder.DropForeignKey(
                name: "FK_Collab_Note_NoteId",
                table: "Collab");

            migrationBuilder.DropIndex(
                name: "IX_Collab_Id",
                table: "Collab");

            migrationBuilder.DropIndex(
                name: "IX_Collab_NoteId",
                table: "Collab");

            migrationBuilder.AddColumn<int>(
                name: "NotesEntityNoteId",
                table: "Collab",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotesUserId",
                table: "Collab",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collab_NotesEntityNoteId",
                table: "Collab",
                column: "NotesEntityNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Collab_NotesUserId",
                table: "Collab",
                column: "NotesUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collab_Note_NotesEntityNoteId",
                table: "Collab",
                column: "NotesEntityNoteId",
                principalTable: "Note",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Collab_User_NotesUserId",
                table: "Collab",
                column: "NotesUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
