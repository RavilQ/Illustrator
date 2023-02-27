using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Illustration.Migrations
{
    public partial class AddConnectionIdToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "AspNetUsers",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastConnectedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastConnectedAt",
                table: "AspNetUsers");
        }
    }
}
