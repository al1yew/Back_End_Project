using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class UpdatedBlogReplyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "BlogCommentReplies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BlogCommentReplies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "BlogCommentReplies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BlogCommentReplies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BlogCommentReplies");

            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "BlogCommentReplies");
        }
    }
}
