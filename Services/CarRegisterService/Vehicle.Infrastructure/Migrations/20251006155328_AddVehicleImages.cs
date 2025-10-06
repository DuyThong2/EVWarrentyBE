using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vehicle_image",
                columns: table => new
                {
                    imageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    caption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Active"),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_image", x => x.imageId);
                    table.ForeignKey(
                        name: "FK_vehicle_image_vehicle_vehicleId",
                        column: x => x.vehicleId,
                        principalTable: "vehicle",
                        principalColumn: "vehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_vehicleimage_vehicle",
                table: "vehicle_image",
                column: "vehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehicle_image");
        }
    }
}
