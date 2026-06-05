using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_Material_20250620_171000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MaterialStockUploadDetail",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MaterialCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Model = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Storage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                StorageDestination = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                RefDoc = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                table.PrimaryKey("PK_MaterialStockUploadDetail", x => x.Id);
                table.ForeignKey(
                    name: "FK_MaterialStockUploadDetail_Materials_RequestId",
                    column: x => x.RequestId,
                    principalTable: "Materials",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MaterialStockUploadDetail_RequestId",
            table: "MaterialStockUploadDetail",
            column: "RequestId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MaterialStockUploadDetail");
    }
}
