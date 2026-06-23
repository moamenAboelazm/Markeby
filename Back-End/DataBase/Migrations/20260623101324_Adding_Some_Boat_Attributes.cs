using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class Adding_Some_Boat_Attributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasToilet",
                table: "Boats");

            migrationBuilder.AddColumn<double>(
                name: "MaxSpeed",
                table: "Boats",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Boats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxSpeed",
                table: "Boats");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Boats");

            migrationBuilder.AddColumn<bool>(
                name: "HasToilet",
                table: "Boats",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
