using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_StockImportPriority_20250715_025430 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "StockImport_Priority",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DPONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                PONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                MateiralCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Qty = table.Column<int>(type: "int", nullable: false),
                Priority = table.Column<int>(type: "int", nullable: true),
                QtyUsed = table.Column<int>(type: "int", nullable: true),
                QtyAvailable = table.Column<int>(type: "int", nullable: true),
                Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImportGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_StockImport_Priority", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "StockImport_Priority");
    }
}
