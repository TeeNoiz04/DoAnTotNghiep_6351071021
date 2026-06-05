using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_StockManagement_20250704_170200 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MaterialStockUploadDetail_Materials_RequestId",
            table: "MaterialStockUploadDetail");

        migrationBuilder.CreateTable(
            name: "MaterialStockUpload",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RequestNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ImportType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                FilName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                table.PrimaryKey("PK_MaterialStockUpload", x => x.Id);
            });

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialStockUploadDetail_MaterialStockUpload_RequestId",
            table: "MaterialStockUploadDetail",
            column: "RequestId",
            principalTable: "MaterialStockUpload",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MaterialStockUploadDetail_MaterialStockUpload_RequestId",
            table: "MaterialStockUploadDetail");

        migrationBuilder.DropTable(
            name: "MaterialStockUpload");

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialStockUploadDetail_Materials_RequestId",
            table: "MaterialStockUploadDetail",
            column: "RequestId",
            principalTable: "Materials",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
