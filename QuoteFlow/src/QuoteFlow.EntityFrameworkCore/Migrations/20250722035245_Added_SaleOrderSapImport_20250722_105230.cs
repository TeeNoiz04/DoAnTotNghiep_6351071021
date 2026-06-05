using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_SaleOrderSapImport_20250722_105230 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SaleOrdersSapImport",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                DONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                DODate = table.Column<DateTime>(type: "datetime2", nullable: true),
                DONote = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                SOSAPNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                DOSAPNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                BillingNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Deleted = table.Column<bool>(type: "bit", nullable: true),
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
                table.PrimaryKey("PK_SaleOrdersSapImport", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SaleOrdersSapImport");
    }
}
