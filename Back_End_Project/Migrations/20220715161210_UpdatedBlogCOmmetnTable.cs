using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class UpdatedBlogCOmmetnTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentAuthor",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "CommentDate",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "CommentText",
                table: "BlogComments");

            migrationBuilder.AddColumn<string>(
                name: "AdminName",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminUserId",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "BlogComments",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "BlogComments",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasResponse",
                table: "BlogComments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsChildComment",
                table: "BlogComments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsParentComment",
                table: "BlogComments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ResponseText",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseTime",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "BlogComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_AdminUserId",
                table: "BlogComments",
                column: "AdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_AspNetUsers_AdminUserId",
                table: "BlogComments",
                column: "AdminUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_AspNetUsers_AdminUserId",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_AdminUserId",
                table: "BlogComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "AdminName",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "HasResponse",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "IsChildComment",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "IsParentComment",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "ResponseText",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "ResponseTime",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "BlogComments");

            migrationBuilder.AddColumn<string>(
                name: "CommentAuthor",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentDate",
                table: "BlogComments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentText",
                table: "BlogComments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
