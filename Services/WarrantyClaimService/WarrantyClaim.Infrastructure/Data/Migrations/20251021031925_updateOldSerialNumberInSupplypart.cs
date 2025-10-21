using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarrantyClaim.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateOldSerialNumberInSupplypart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldPartSerialNumber",
                table: "partSuply",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPartSerialNumber",
                table: "partSuply");
        }
    }
}
