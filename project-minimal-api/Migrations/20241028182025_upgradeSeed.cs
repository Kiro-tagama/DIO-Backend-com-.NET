using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_minimal_api.Migrations
{
    /// <inheritdoc />
    public partial class upgradeSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: 1,
                column: "Profile",
                value: "Adm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: 1,
                column: "Profile",
                value: "admin");
        }
    }
}
