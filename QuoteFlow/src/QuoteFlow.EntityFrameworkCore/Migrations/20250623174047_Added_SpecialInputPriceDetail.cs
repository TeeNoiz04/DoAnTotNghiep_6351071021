using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_SpecialInputPriceDetail : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SpecialInputPriceDetail",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SpecialInputPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                MaterialCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Model = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                Spec1 = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                LimitQty = table.Column<int>(type: "int", nullable: true),
                InputPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                LandedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Used = table.Column<int>(type: "int", nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                table.PrimaryKey("PK_SpecialInputPriceDetail", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SpecialInputPriceDetail");
    }
}
