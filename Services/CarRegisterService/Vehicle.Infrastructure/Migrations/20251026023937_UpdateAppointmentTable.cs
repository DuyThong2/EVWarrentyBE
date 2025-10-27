using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_appointment_scheduleddate",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "scheduledDate",
                table: "appointment");

            migrationBuilder.RenameColumn(
                name: "scheduledTime",
                table: "appointment",
                newName: "scheduledDateTime");

            migrationBuilder.CreateIndex(
                name: "ix_appointment_scheduleddatetime",
                table: "appointment",
                column: "scheduledDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_appointment_scheduleddatetime",
                table: "appointment");

            migrationBuilder.RenameColumn(
                name: "scheduledDateTime",
                table: "appointment",
                newName: "scheduledTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "scheduledDate",
                table: "appointment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "ix_appointment_scheduleddate",
                table: "appointment",
                column: "scheduledDate");
        }
    }
}
