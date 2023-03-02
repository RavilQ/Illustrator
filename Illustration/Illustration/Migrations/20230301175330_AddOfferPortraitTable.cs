using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Illustration.Migrations
{
    public partial class AddOfferPortraitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfferPortraits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PortraitId = table.Column<int>(type: "int", nullable: false),
                    fivePercentPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferPortraits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferPortraits_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferPortraits_Portraits_PortraitId",
                        column: x => x.PortraitId,
                        principalTable: "Portraits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferPortraits_AppUserId",
                table: "OfferPortraits",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferPortraits_PortraitId",
                table: "OfferPortraits",
                column: "PortraitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferPortraits");
        }
    }
}
