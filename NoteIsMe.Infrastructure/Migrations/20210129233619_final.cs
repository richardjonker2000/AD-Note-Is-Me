using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteIsMe.Infrastructure.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(maxLength: 1000, nullable: false),
                    Password = table.Column<string>(maxLength: 1000, nullable: false),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    IconURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notebooks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notebooks_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FolderNotebooks",
                columns: table => new
                {
                    NoteBookId = table.Column<int>(nullable: false),
                    FolderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderNotebooks", x => new { x.FolderId, x.NoteBookId });
                    table.ForeignKey(
                        name: "FK_FolderNotebooks_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderNotebooks_Notebooks_NoteBookId",
                        column: x => x.NoteBookId,
                        principalTable: "Notebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    NotebookId = table.Column<int>(nullable: false),
                    ViewPermission = table.Column<bool>(nullable: false),
                    EditPermission = table.Column<bool>(nullable: false),
                    SharePermission = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => new { x.UserId, x.NotebookId });
                    table.ForeignKey(
                        name: "FK_Groups_Notebooks_NotebookId",
                        column: x => x.NotebookId,
                        principalTable: "Notebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Groups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    NotebookId = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Notebooks_NotebookId",
                        column: x => x.NotebookId,
                        principalTable: "Notebooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notes_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sketches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    NotebookId = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sketches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sketches_Notebooks_NotebookId",
                        column: x => x.NotebookId,
                        principalTable: "Notebooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sketches_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NoteTags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false),
                    NoteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTags", x => new { x.NoteId, x.TagId });
                    table.ForeignKey(
                        name: "FK_NoteTags_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SketchTags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false),
                    SketchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SketchTags", x => new { x.SketchId, x.TagId });
                    table.ForeignKey(
                        name: "FK_SketchTags_Sketches_SketchId",
                        column: x => x.SketchId,
                        principalTable: "Sketches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SketchTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role", "Salt" },
                values: new object[] { 1, "admin@admin.com", "admin", "1zbbwGguKx3wT7UHPOTHjS1mCEaj5xriQAEqKQQQlVw=", 3, "/zzRmyEbyhaZfi+SCExAeweJFih4D8aZ/uL8XYy03dE=" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role", "Salt" },
                values: new object[] { 2, "user1@example.com", "User One", "NeP5cLGOuXtXI4YL0zbY27Nrawu8/SRS4RDs4LRshZ4=", 1, "F9wZzYqCFH/RiIPMqT4x/SDXkb7plRkwT6LM74xJ3FE=" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role", "Salt" },
                values: new object[] { 3, "user2@example.com", "User Two", "yttEU+VeADGQ3zv4911feTo1EyA2B9dfl8WH8ppbv1I=", 1, "RoFZqkf6zcV2WFs/3sfXze58KmDY1vumxRYOht5w/nQ=" });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "IconURL", "Name", "OwnerId" },
                values: new object[,]
                {
                    { 1, "/Assets/folderIcons/home.png", "Folder", 1 },
                    { 2, "/Assets/folderIcons/school.png", "School Item", 2 },
                    { 3, "/Assets/folderIcons/misc.png", "My Misc", 2 },
                    { 4, "/Assets/folderIcons/work.png", "Only One", 3 }
                });

            migrationBuilder.InsertData(
                table: "Notebooks",
                columns: new[] { "Id", "OwnerId", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Only Mine" },
                    { 2, 2, "Shared NB" },
                    { 3, 2, "Stuff" },
                    { 4, 3, "Everyone's NB" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Color", "Name", "UserId" },
                values: new object[] { 1, "#8c34eb", "App Dev", 1 });

            migrationBuilder.InsertData(
                table: "FolderNotebooks",
                columns: new[] { "FolderId", "NoteBookId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "UserId", "NotebookId", "EditPermission", "SharePermission", "ViewPermission" },
                values: new object[,]
                {
                    { 3, 2, true, true, true },
                    { 1, 4, false, true, true },
                    { 2, 4, true, false, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FolderNotebooks_NoteBookId",
                table: "FolderNotebooks",
                column: "NoteBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_OwnerId",
                table: "Folders",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_NotebookId",
                table: "Groups",
                column: "NotebookId");

            migrationBuilder.CreateIndex(
                name: "IX_Notebooks_OwnerId",
                table: "Notebooks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_NotebookId",
                table: "Notes",
                column: "NotebookId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_OwnerId",
                table: "Notes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTags_TagId",
                table: "NoteTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Sketches_NotebookId",
                table: "Sketches",
                column: "NotebookId");

            migrationBuilder.CreateIndex(
                name: "IX_Sketches_OwnerId",
                table: "Sketches",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SketchTags_TagId",
                table: "SketchTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UserId",
                table: "Tags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderNotebooks");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "NoteTags");

            migrationBuilder.DropTable(
                name: "SketchTags");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Sketches");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Notebooks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
