using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class UpdatedProductReviewTable_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewValue",
                table: "ProductReviews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewValue",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
