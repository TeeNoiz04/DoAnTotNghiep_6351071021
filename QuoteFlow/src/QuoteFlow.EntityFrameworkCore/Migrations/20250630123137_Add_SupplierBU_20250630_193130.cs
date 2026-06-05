using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Add_SupplierBU_20250630_193130 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Supplier_BU",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SupplierBUCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                SupplierBURemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                OrderMethod = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                POTemplate = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                Contact = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                INCOTerm = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                PaymentTermCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                PaymentDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SupplierCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                SupplierShortName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                SupplierAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                SortOrder = table.Column<int>(type: "int", nullable: false),
                FASCMVendorCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                FASCMBuyerCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                FASCMConsigneeCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                FASCMSectionCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                FASCMPaymentTerm = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                FASCMFreightMethod = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                FASCMDeliveryTerms = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                FASCMPlaceOfDeliveryTerms = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                FASCMShippingMarkCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                table.PrimaryKey("PK_Supplier_BU", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Supplier_BU");
    }
}
