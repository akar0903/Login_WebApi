using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class collabb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collab",
                columns: table => new
                {
                    CollabId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CollabEmail = table.Column<string>(nullable: true),
                    IsTrash = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    NotesUserId = table.Column<int>(nullable: true),
                    NoteId = table.Column<int>(nullable: false),
                    NotesEntityNoteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collab", x => x.CollabId);
                    table.ForeignKey(
                        name: "FK_Collab_Note_NotesEntityNoteId",
                        column: x => x.NotesEntityNoteId,
                        principalTable: "Note",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Collab_User_NotesUserId",
                        column: x => x.NotesUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collab_NotesEntityNoteId",
                table: "Collab",
                column: "NotesEntityNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Collab_NotesUserId",
                table: "Collab",
                column: "NotesUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collab");
        }
    }
}
