using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    customerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Active"),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.customerId);
                });

            migrationBuilder.CreateTable(
                name: "vehicle",
                columns: table => new
                {
                    vehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vin = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    plateNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    trim = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    modelYear = table.Column<int>(type: "int", nullable: false),
                    color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    distanceMeter = table.Column<long>(type: "bigint", nullable: true),
                    purchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    warrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    warrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Active"),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle", x => x.vehicleId);
                    table.ForeignKey(
                        name: "FK_vehicle_customer_customerId",
                        column: x => x.customerId,
                        principalTable: "customer",
                        principalColumn: "customerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_part",
                columns: table => new
                {
                    partId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    serialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    partType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    batchCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    installedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    warrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    warrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Installed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_part", x => x.partId);
                    table.ForeignKey(
                        name: "FK_vehicle_part_vehicle_vehicleId",
                        column: x => x.vehicleId,
                        principalTable: "vehicle",
                        principalColumn: "vehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "customer",
                columns: new[] { "customerId", "address", "createdAt", "email", "fullName", "phone", "updatedAt" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Hanoi, Vietnam", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nguyenvana@example.com", "Nguyen Van A", "0912345678", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "vehicle",
                columns: new[] { "vehicleId", "color", "createdAt", "customerId", "distanceMeter", "model", "modelYear", "plateNumber", "purchaseDate", "trim", "updatedAt", "vin", "warrantyEndDate", "warrantyStartDate" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Black", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 15000L, "Honda City", 2022, "30A-12345", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "RS", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1HGCM82633A123456", new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "vehicle_part",
                columns: new[] { "partId", "batchCode", "installedAt", "partType", "serialNumber", "vehicleId", "warrantyEndDate", "warrantyStartDate" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "BATCH-2023", new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BATTERY", "BAT-123456789", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "ix_customer_email",
                table: "customer",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "ix_customer_phone",
                table: "customer",
                column: "phone");

            migrationBuilder.CreateIndex(
                name: "ix_vehicle_customer",
                table: "vehicle",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_vin",
                table: "vehicle",
                column: "vin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_part_parttype",
                table: "vehicle_part",
                column: "partType");

            migrationBuilder.CreateIndex(
                name: "ix_part_vehicle",
                table: "vehicle_part",
                column: "vehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_part_serialNumber",
                table: "vehicle_part",
                column: "serialNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehicle_part");

            migrationBuilder.DropTable(
                name: "vehicle");

            migrationBuilder.DropTable(
                name: "customer");
        }
    }
}
