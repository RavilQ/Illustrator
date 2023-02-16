using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Illustration.Migrations
{
    public partial class AddPortraitIdToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PortraitId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PortraitId",
                table: "Orders",
                column: "PortraitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Portraits_PortraitId",
                table: "Orders",
                column: "PortraitId",
                principalTable: "Portraits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Portraits_PortraitId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PortraitId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PortraitId",
                table: "Orders");
        }
    }
}
