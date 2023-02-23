using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Illustration.Migrations
{
    public partial class AddMemberCheckToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasMember",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasMember",
                table: "AspNetUsers");
        }
    }
}
