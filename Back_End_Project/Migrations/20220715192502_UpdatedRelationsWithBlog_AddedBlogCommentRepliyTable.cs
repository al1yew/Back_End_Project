using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class UpdatedRelationsWithBlog_AddedBlogCommentRepliyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminImage",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "AdminName",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "IsChildComment",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "ResponseText",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "ResponseTime",
                table: "BlogComments");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlogCommentReply",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminImage = table.Column<string>(nullable: true),
                    AdminName = table.Column<string>(nullable: true),
                    ResponseTime = table.Column<DateTime>(nullable: true),
                    ResponseText = table.Column<string>(nullable: true),
                    IsChildComment = table.Column<bool>(nullable: false),
                    BlogCommentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCommentReply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogCommentReply_BlogComments_BlogCommentId",
                        column: x => x.BlogCommentId,
                        principalTable: "BlogComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogCommentReply_BlogCommentId",
                table: "BlogCommentReply",
                column: "BlogCommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogCommentReply");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "BlogComments");

            migrationBuilder.AddColumn<string>(
                name: "AdminImage",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminName",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsChildComment",
                table: "BlogComments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ResponseText",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseTime",
                table: "BlogComments",
                type: "datetime2",
                nullable: true);
        }
    }
}
