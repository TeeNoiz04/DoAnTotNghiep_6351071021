using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_DistributorTarget_20250723_164630 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DistributorTarget",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BuyerTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                BuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BuyerCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                FinanceYear = table.Column<int>(type: "int", nullable: true),
                FirstFYTarget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                SecondFYTarget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                table.PrimaryKey("PK_DistributorTarget", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DistributorTarget");
    }
}
