using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarrantyClaim.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_FK_SetNull_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_partSuply_claimItem_claimItemId",
                table: "partSuply");

            migrationBuilder.DropForeignKey(
                name: "FK_workorder_claimItem_claimItemId",
                table: "workorder");

            migrationBuilder.AlterColumn<Guid>(
                name: "technicianId",
                table: "workorder",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "claimItemId",
                table: "workorder",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "claimItemId",
                table: "partSuply",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_claim_staffId",
                table: "claim",
                column: "staffId");

            migrationBuilder.AddForeignKey(
                name: "FK_claim_technicians_staffId",
                table: "claim",
                column: "staffId",
                principalTable: "technicians",
                principalColumn: "technicianId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_partSuply_claimItem_claimItemId",
                table: "partSuply",
                column: "claimItemId",
                principalTable: "claimItem",
                principalColumn: "claimItemId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_workorder_claimItem_claimItemId",
                table: "workorder",
                column: "claimItemId",
                principalTable: "claimItem",
                principalColumn: "claimItemId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_claim_technicians_staffId",
                table: "claim");

            migrationBuilder.DropForeignKey(
                name: "FK_partSuply_claimItem_claimItemId",
                table: "partSuply");

            migrationBuilder.DropForeignKey(
                name: "FK_workorder_claimItem_claimItemId",
                table: "workorder");

            migrationBuilder.DropIndex(
                name: "IX_claim_staffId",
                table: "claim");

            migrationBuilder.AlterColumn<Guid>(
                name: "technicianId",
                table: "workorder",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "claimItemId",
                table: "workorder",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "claimItemId",
                table: "partSuply",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_partSuply_claimItem_claimItemId",
                table: "partSuply",
                column: "claimItemId",
                principalTable: "claimItem",
                principalColumn: "claimItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_workorder_claimItem_claimItemId",
                table: "workorder",
                column: "claimItemId",
                principalTable: "claimItem",
                principalColumn: "claimItemId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
