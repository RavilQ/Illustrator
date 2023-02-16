using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Illustration.Migrations
{
    public partial class DeleteReviewWriterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ReviewWriters_ReviewWriterId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "ReviewWriters");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewWriterId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewWriterId",
                table: "Reviews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewWriterId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReviewWriters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewWriters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewWriterId",
                table: "Reviews",
                column: "ReviewWriterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ReviewWriters_ReviewWriterId",
                table: "Reviews",
                column: "ReviewWriterId",
                principalTable: "ReviewWriters",
                principalColumn: "Id");
        }
    }
}
