using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarrantyHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarrantyHistories",
                columns: table => new
                {
                    HistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClaimId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PerformedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceCenterName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarrantyHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_WarrantyHistories_vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "vehicle",
                        principalColumn: "vehicleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarrantyHistories_vehicle_part_PartId",
                        column: x => x.PartId,
                        principalTable: "vehicle_part",
                        principalColumn: "partId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_ClaimId",
                table: "WarrantyHistories",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_CreatedAt",
                table: "WarrantyHistories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_EventType",
                table: "WarrantyHistories",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_PartId",
                table: "WarrantyHistories",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_PerformedBy",
                table: "WarrantyHistories",
                column: "PerformedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_PolicyId",
                table: "WarrantyHistories",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_Status",
                table: "WarrantyHistories",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_WarrantyHistories_VehicleId",
                table: "WarrantyHistories",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarrantyHistories");
        }
    }
}
