using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Illustration.Migrations
{
    public partial class AddBlogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstImage = table.Column<string>(type: "nvarchar(170)", maxLength: 170, nullable: true),
                    FirstTitle = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    FirstText = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    SecondImage = table.Column<string>(type: "nvarchar(170)", maxLength: 170, nullable: true),
                    SecondTitle = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SecondText = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    FirstGreyText = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    FirstGreyAuthor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ThirdImage = table.Column<string>(type: "nvarchar(170)", maxLength: 170, nullable: true),
                    ThirdTitle = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ThirdText = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    SecondGreyText = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SecondGreyAuthor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog");
        }
    }
}
