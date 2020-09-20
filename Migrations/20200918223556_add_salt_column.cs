using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pempo_backend.Migrations
{
    public partial class add_salt_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "tblAdmin",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "tblAdmin");
        }
    }
}
