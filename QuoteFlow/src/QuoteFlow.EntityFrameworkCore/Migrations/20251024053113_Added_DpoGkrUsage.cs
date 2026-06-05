using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Added_DpoGkrUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DpoGkrUsage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GkrId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DpoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GkrNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DpoNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GkrDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DpoDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GolfaCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GkrQty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DpoQty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GkrLockStockQty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DpoLockStockQty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GkrLockShipmentQty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DpoLockShipmentQty = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DpoGkrUsage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DpoGkrUsage_DpoDetailId",
                table: "DpoGkrUsage",
                column: "DpoDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DpoGkrUsage_DpoId",
                table: "DpoGkrUsage",
                column: "DpoId");

            migrationBuilder.CreateIndex(
                name: "IX_DpoGkrUsage_GkrDetailId",
                table: "DpoGkrUsage",
                column: "GkrDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DpoGkrUsage_GkrId",
                table: "DpoGkrUsage",
                column: "GkrId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DpoGkrUsage");
        }
    }
}
