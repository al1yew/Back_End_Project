using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class UpdatedProductReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductReviews",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "ProductReviews",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "ProductReviews");
        }
    }
}
