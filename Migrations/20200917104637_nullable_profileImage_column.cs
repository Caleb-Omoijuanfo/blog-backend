using Microsoft.EntityFrameworkCore.Migrations;

namespace Pempo_backend.Migrations
{
    public partial class nullable_profileImage_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "tblAdmin");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "tblAdmin",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "tblAdmin");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "tblAdmin",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
