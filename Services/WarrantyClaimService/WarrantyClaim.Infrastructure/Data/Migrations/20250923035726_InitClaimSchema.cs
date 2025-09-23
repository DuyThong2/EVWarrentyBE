using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarrantyClaim.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitClaimSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "claim",
                columns: table => new
                {
                    claimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    staffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VIN = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    distanceMeter = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    fileURL = table.Column<string>(type: "text", nullable: true),
                    totalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    claimType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "SUBMITTED"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim", x => x.claimId);
                });

            migrationBuilder.CreateTable(
                name: "technicians",
                columns: table => new
                {
                    technicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    staffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_technicians", x => x.technicianId);
                });

            migrationBuilder.CreateTable(
                name: "claimItem",
                columns: table => new
                {
                    claimItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    claimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    partSerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    payAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    paidBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    imgURLs = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "PENDING"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claimItem", x => x.claimItemId);
                    table.ForeignKey(
                        name: "FK_claimItem_claim_claimId",
                        column: x => x.claimId,
                        principalTable: "claim",
                        principalColumn: "claimId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partSuply",
                columns: table => new
                {
                    partSuplyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    claimItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    partId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    newSerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    shipmentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    shipmentRef = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "REQUESTED"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partSuply", x => x.partSuplyId);
                    table.ForeignKey(
                        name: "FK_partSuply_claimItem_claimItemId",
                        column: x => x.claimItemId,
                        principalTable: "claimItem",
                        principalColumn: "claimItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workorder",
                columns: table => new
                {
                    workorderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    claimItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    technicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    workingHours = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "OPEN"),
                    startedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workorder", x => x.workorderId);
                    table.ForeignKey(
                        name: "FK_workorder_claimItem_claimItemId",
                        column: x => x.claimItemId,
                        principalTable: "claimItem",
                        principalColumn: "claimItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_workorder_technicians_technicianId",
                        column: x => x.technicianId,
                        principalTable: "technicians",
                        principalColumn: "technicianId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_claimItem_claimId",
                table: "claimItem",
                column: "claimId");

            migrationBuilder.CreateIndex(
                name: "IX_partSuply_claimItemId",
                table: "partSuply",
                column: "claimItemId");

            migrationBuilder.CreateIndex(
                name: "IX_workorder_claimItemId",
                table: "workorder",
                column: "claimItemId");

            migrationBuilder.CreateIndex(
                name: "IX_workorder_technicianId",
                table: "workorder",
                column: "technicianId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partSuply");

            migrationBuilder.DropTable(
                name: "workorder");

            migrationBuilder.DropTable(
                name: "claimItem");

            migrationBuilder.DropTable(
                name: "technicians");

            migrationBuilder.DropTable(
                name: "claim");
        }
    }
}
