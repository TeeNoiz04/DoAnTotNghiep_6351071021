using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_DPO_20250612_184000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DPO",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DPONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DPOType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                DPOSubType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CostCenter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                BuyerTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                BuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                BuyerShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Remark = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
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
                table.PrimaryKey("PK_DPO", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DPO");
    }
}
