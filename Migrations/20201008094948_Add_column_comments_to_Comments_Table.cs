using Microsoft.EntityFrameworkCore.Migrations;

namespace Pempo_backend.Migrations
{
    public partial class Add_column_comments_to_Comments_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "tblComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "tblComments");
        }
    }
}
