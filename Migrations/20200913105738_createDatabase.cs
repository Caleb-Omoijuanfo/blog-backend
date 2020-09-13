using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pempo_backend.Migrations
{
    public partial class createDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAdmin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    ProfileImage = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAdmin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblPost",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    AdminId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblPost_tblAdmin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "tblAdmin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    PostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblComments_tblPost_PostId",
                        column: x => x.PostId,
                        principalTable: "tblPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblLikes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    PostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblLikes_tblPost_PostId",
                        column: x => x.PostId,
                        principalTable: "tblPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblMedia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    MediaData = table.Column<byte[]>(nullable: true),
                    PostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblMedia_tblPost_PostId",
                        column: x => x.PostId,
                        principalTable: "tblPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblComments_PostId",
                table: "tblComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_tblLikes_PostId",
                table: "tblLikes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMedia_PostId",
                table: "tblMedia",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_tblPost_AdminId",
                table: "tblPost",
                column: "AdminId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblComments");

            migrationBuilder.DropTable(
                name: "tblLikes");

            migrationBuilder.DropTable(
                name: "tblMedia");

            migrationBuilder.DropTable(
                name: "tblPost");

            migrationBuilder.DropTable(
                name: "tblAdmin");
        }
    }
}
