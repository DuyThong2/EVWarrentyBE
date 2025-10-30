using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    CateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CateCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CateName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.CateId);
                });

            migrationBuilder.CreateTable(
                name: "package",
                columns: table => new
                {
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 1m),
                    Note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_package", x => x.PackageId);
                    table.ForeignKey(
                        name: "FK_package_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "CateId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "part",
                columns: table => new
                {
                    partId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "ACTIVE"),
                    CateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part", x => x.partId);
                    table.ForeignKey(
                        name: "FK_part_category_CateId",
                        column: x => x.CateId,
                        principalTable: "category",
                        principalColumn: "CateId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_part_package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "package",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "warranty_policy",
                columns: table => new
                {
                    PolicyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    WarrantyDuration = table.Column<int>(type: "int", nullable: true),
                    WarrantyDistance = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warranty_policy", x => x.PolicyId);
                    table.ForeignKey(
                        name: "FK_warranty_policy_package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "package",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_CateCode",
                table: "category",
                column: "CateCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_category_name",
                table: "category",
                column: "CateName");

            migrationBuilder.CreateIndex(
                name: "IX_package_CategoryId",
                table: "package",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "ix_package_name",
                table: "package",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_package_PackageCode",
                table: "package",
                column: "PackageCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_part_CateId",
                table: "part",
                column: "CateId");

            migrationBuilder.CreateIndex(
                name: "ix_part_name",
                table: "part",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_part_PackageId",
                table: "part",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "ix_policy_pkg_status",
                table: "warranty_policy",
                columns: new[] { "PackageId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_warranty_policy_Code",
                table: "warranty_policy",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "part");

            migrationBuilder.DropTable(
                name: "warranty_policy");

            migrationBuilder.DropTable(
                name: "package");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
