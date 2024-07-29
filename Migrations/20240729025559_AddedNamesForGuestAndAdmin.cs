using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntroBE.Migrations
{
    /// <inheritdoc />
    public partial class AddedNamesForGuestAndAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "GuestList",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "GuestList",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AdminList",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AdminList",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "GuestList");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "GuestList");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AdminList");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AdminList");
        }
    }
}
