using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class UpdatedBlogCommentTable_addedAuthorImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Website",
                table: "BlogComments");

            migrationBuilder.AddColumn<string>(
                name: "AuthorImage",
                table: "BlogComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorImage",
                table: "BlogComments");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
