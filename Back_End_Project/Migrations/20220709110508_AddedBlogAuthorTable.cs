using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class AddedBlogAuthorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogAuthorId",
                table: "Blogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BlogAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorImage = table.Column<string>(maxLength: 255, nullable: true),
                    AuthorName = table.Column<string>(maxLength: 255, nullable: true),
                    AuthorPosition = table.Column<string>(maxLength: 255, nullable: true),
                    LinkedInUrl = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogAuthors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogAuthorId",
                table: "Blogs",
                column: "BlogAuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogAuthors_BlogAuthorId",
                table: "Blogs",
                column: "BlogAuthorId",
                principalTable: "BlogAuthors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogAuthors_BlogAuthorId",
                table: "Blogs");

            migrationBuilder.DropTable(
                name: "BlogAuthors");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_BlogAuthorId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "BlogAuthorId",
                table: "Blogs");
        }
    }
}
