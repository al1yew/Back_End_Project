using Microsoft.EntityFrameworkCore.Migrations;

namespace Back_End_Project.Migrations
{
    public partial class UpdatedHomeBannerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "HomeBanners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "HomeBanners");
        }
    }
}
