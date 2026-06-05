using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250829_114600 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SpecialInputPrice_SpecialInputPriceId",
            table: "PriceOffer");

        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SystemCategories_BuyerTypeId",
            table: "PriceOffer");

        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SystemCategories_EUIndustryId",
            table: "PriceOffer");

        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SystemCategories_KeyAccountClassId",
            table: "PriceOffer");

        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SystemCategories_KeyAccountTypeId",
            table: "PriceOffer");

        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SystemCategories_LocationId",
            table: "PriceOffer");

        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SystemCategories_ProjectTypeId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_BuyerTypeId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_EUIndustryId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_KeyAccountClassId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_KeyAccountTypeId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_LocationId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_ProjectTypeId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_SpecialInputPriceId",
            table: "PriceOffer");

        migrationBuilder.AddColumn<string>(
            name: "BuyerTypeDescription",
            table: "PriceOffer",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "AmountIncludeExtraFee",
            table: "DPODetail",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "BuyerTypeDescription",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "AmountIncludeExtraFee",
            table: "DPODetail");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_BuyerTypeId",
            table: "PriceOffer",
            column: "BuyerTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_EUIndustryId",
            table: "PriceOffer",
            column: "EUIndustryId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_KeyAccountClassId",
            table: "PriceOffer",
            column: "KeyAccountClassId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_KeyAccountTypeId",
            table: "PriceOffer",
            column: "KeyAccountTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_LocationId",
            table: "PriceOffer",
            column: "LocationId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_ProjectTypeId",
            table: "PriceOffer",
            column: "ProjectTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_SpecialInputPriceId",
            table: "PriceOffer",
            column: "SpecialInputPriceId");

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_SpecialInputPrice_SpecialInputPriceId",
            table: "PriceOffer",
            column: "SpecialInputPriceId",
            principalTable: "SpecialInputPrice",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_SystemCategories_BuyerTypeId",
            table: "PriceOffer",
            column: "BuyerTypeId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_SystemCategories_EUIndustryId",
            table: "PriceOffer",
            column: "EUIndustryId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_SystemCategories_KeyAccountClassId",
            table: "PriceOffer",
            column: "KeyAccountClassId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_SystemCategories_KeyAccountTypeId",
            table: "PriceOffer",
            column: "KeyAccountTypeId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_SystemCategories_LocationId",
            table: "PriceOffer",
            column: "LocationId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_SystemCategories_ProjectTypeId",
            table: "PriceOffer",
            column: "ProjectTypeId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
