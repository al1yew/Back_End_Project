using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class UpdatedBlogRelationsTables_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogCommentReply_BlogComments_BlogCommentId",
                table: "BlogCommentReply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogCommentReply",
                table: "BlogCommentReply");

            migrationBuilder.RenameTable(
                name: "BlogCommentReply",
                newName: "BlogCommentReplies");

            migrationBuilder.RenameIndex(
                name: "IX_BlogCommentReply_BlogCommentId",
                table: "BlogCommentReplies",
                newName: "IX_BlogCommentReplies_BlogCommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogCommentReplies",
                table: "BlogCommentReplies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogCommentReplies_BlogComments_BlogCommentId",
                table: "BlogCommentReplies",
                column: "BlogCommentId",
                principalTable: "BlogComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogCommentReplies_BlogComments_BlogCommentId",
                table: "BlogCommentReplies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogCommentReplies",
                table: "BlogCommentReplies");

            migrationBuilder.RenameTable(
                name: "BlogCommentReplies",
                newName: "BlogCommentReply");

            migrationBuilder.RenameIndex(
                name: "IX_BlogCommentReplies_BlogCommentId",
                table: "BlogCommentReply",
                newName: "IX_BlogCommentReply_BlogCommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogCommentReply",
                table: "BlogCommentReply",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogCommentReply_BlogComments_BlogCommentId",
                table: "BlogCommentReply",
                column: "BlogCommentId",
                principalTable: "BlogComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
