using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehiclePartAndWarrantyHistoryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarrantyHistories_PolicyId",
                table: "WarrantyHistories");

            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "WarrantyHistories");

            migrationBuilder.DropColumn(
                name: "ServiceCenterName",
                table: "WarrantyHistories");

            migrationBuilder.AddColumn<long>(
                name: "WarrantyDistance",
                table: "WarrantyHistories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "vehicle_part",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "warrantyDistance",
                table: "vehicle_part",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "vehicle_part",
                keyColumn: "partId",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                columns: new[] { "code", "warrantyDistance" },
                values: new object[] { "BAT-001", 50000L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarrantyDistance",
                table: "WarrantyHistories");

            migrationBuilder.DropColumn(
                name: "code",
                table: "vehicle_part");

            migrationBuilder.DropColumn(
                name: "warrantyDistance",
                table: "vehicle_part");

            migrationBuilder.AddColumn<Guid>(
                name: "PolicyId",
                table: "WarrantyHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceCenterName",
                table: "WarrantyHistories",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_PolicyId",
                table: "WarrantyHistories",
                column: "PolicyId");
        }
    }
}
