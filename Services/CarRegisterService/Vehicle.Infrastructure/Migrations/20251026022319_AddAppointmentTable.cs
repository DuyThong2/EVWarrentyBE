using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "appointment",
                columns: table => new
                {
                    appointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    scheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    scheduledTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    appointmentType = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Other"),
                    status = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValue: "Scheduled"),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment", x => x.appointmentId);
                    table.ForeignKey(
                        name: "FK_appointment_vehicle_vehicleId",
                        column: x => x.vehicleId,
                        principalTable: "vehicle",
                        principalColumn: "vehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_appointment_scheduleddate",
                table: "appointment",
                column: "scheduledDate");

            migrationBuilder.CreateIndex(
                name: "ix_appointment_status",
                table: "appointment",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_appointment_vehicle",
                table: "appointment",
                column: "vehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointment");
        }
    }
}
