using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class UpdatedBlogAUthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BlogAuthors",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "BlogAuthors",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BlogAuthors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "BlogAuthors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "BlogAuthors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BlogAuthors");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "BlogAuthors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BlogAuthors");

            migrationBuilder.DropColumn(
                name: "IsUpdated",
                table: "BlogAuthors");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BlogAuthors");
        }
    }
}
