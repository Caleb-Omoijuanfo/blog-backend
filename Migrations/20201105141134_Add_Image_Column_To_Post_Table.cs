using Microsoft.EntityFrameworkCore.Migrations;

namespace Pempo_backend.Migrations
{
    public partial class Add_Image_Column_To_Post_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeaturedImage",
                table: "tblPost",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeaturedImage",
                table: "tblPost");
        }
    }
}
