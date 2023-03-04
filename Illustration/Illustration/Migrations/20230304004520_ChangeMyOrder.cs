using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Illustration.Migrations
{
    public partial class ChangeMyOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Orders",
                newName: "Fullname");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Orders",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70);

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "Orders",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Orders",
                type: "nvarchar(170)",
                maxLength: 170,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(170)",
                oldMaxLength: 170);

            migrationBuilder.AlterColumn<string>(
                name: "AditionalInformation",
                table: "Orders",
                type: "nvarchar(370)",
                maxLength: 370,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(370)",
                oldMaxLength: 370);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(270)",
                maxLength: 270,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(270)",
                oldMaxLength: 270);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "Orders",
                newName: "LastName");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Orders",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "Orders",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Orders",
                type: "nvarchar(170)",
                maxLength: 170,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(170)",
                oldMaxLength: 170,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AditionalInformation",
                table: "Orders",
                type: "nvarchar(370)",
                maxLength: 370,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(370)",
                oldMaxLength: 370,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(270)",
                maxLength: 270,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(270)",
                oldMaxLength: 270,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Orders",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Phone",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
