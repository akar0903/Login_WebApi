using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class arun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLabel_User_Id",
                table: "UserLabel");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLabel_Note_NotesId",
                table: "UserLabel");

            migrationBuilder.DropIndex(
                name: "IX_UserLabel_Id",
                table: "UserLabel");

            migrationBuilder.DropIndex(
                name: "IX_UserLabel_NotesId",
                table: "UserLabel");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserLabel");

            migrationBuilder.DropColumn(
                name: "NotesId",
                table: "UserLabel");

            migrationBuilder.AddColumn<int>(
                name: "NoteId",
                table: "UserLabel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserLabel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserLabel_NoteId",
                table: "UserLabel",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLabel_UserId",
                table: "UserLabel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLabel_Note_NoteId",
                table: "UserLabel",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLabel_User_UserId",
                table: "UserLabel",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLabel_Note_NoteId",
                table: "UserLabel");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLabel_User_UserId",
                table: "UserLabel");

            migrationBuilder.DropIndex(
                name: "IX_UserLabel_NoteId",
                table: "UserLabel");

            migrationBuilder.DropIndex(
                name: "IX_UserLabel_UserId",
                table: "UserLabel");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "UserLabel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserLabel");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserLabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotesId",
                table: "UserLabel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserLabel_Id",
                table: "UserLabel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLabel_NotesId",
                table: "UserLabel",
                column: "NotesId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLabel_User_Id",
                table: "UserLabel",
                column: "Id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLabel_Note_NotesId",
                table: "UserLabel",
                column: "NotesId",
                principalTable: "Note",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
