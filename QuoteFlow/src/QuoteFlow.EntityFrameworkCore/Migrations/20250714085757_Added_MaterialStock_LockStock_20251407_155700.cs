using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_MaterialStock_LockStock_20251407_155700 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MaterialStock_LockStock",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GolfaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DPODetail_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                StockCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Qty = table.Column<int>(type: "int", nullable: true),
                Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReleasedLock = table.Column<byte>(type: "tinyint", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialStock_LockStock", x => x.Id);
                table.ForeignKey(
                    name: "FK_MaterialStock_LockStock_DPODetail_DPODetail_Id",
                    column: x => x.DPODetail_Id,
                    principalTable: "DPODetail",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_MaterialStock_LockStock_StockCategory_StockCategoryId",
                    column: x => x.StockCategoryId,
                    principalTable: "StockCategory",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_MaterialStock_LockStock_DPODetail_Id",
            table: "MaterialStock_LockStock",
            column: "DPODetail_Id");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialStock_LockStock_StockCategoryId",
            table: "MaterialStock_LockStock",
            column: "StockCategoryId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MaterialStock_LockStock");
    }
}
