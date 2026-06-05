using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Removed_Distributor_20250615_115600 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_Distributors_BuyerId",
            table: "KeyAccount");

        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_Buyer_DistributorId",
            table: "PriceOffer");

        migrationBuilder.DropForeignKey(
            name: "FK_SaleTeam_Distributors_BuyerId",
            table: "SaleTeam");

        migrationBuilder.DropTable(
            name: "Distributors");

        migrationBuilder.RenameColumn(
            name: "DistributorPrice",
            table: "PriceOfferDetail",
            newName: "BuyerPrice");

        migrationBuilder.RenameColumn(
            name: "DistributorId",
            table: "PriceOffer",
            newName: "BuyerId");

        migrationBuilder.RenameIndex(
            name: "IX_PriceOffer_DistributorId",
            table: "PriceOffer",
            newName: "IX_PriceOffer_BuyerId");

        //migrationBuilder.CreateIndex(
        //    name: "IX_PriceOffer_BuyerId",
        //    table: "PriceOffer",
        //    column: "BuyerId");

        migrationBuilder.RenameColumn(
            name: "Distributor_Info2",
            table: "KeyAccount_Evaluations",
            newName: "Buyer_Info2");

        migrationBuilder.RenameColumn(
            name: "Distributor_Info1",
            table: "KeyAccount_Evaluations",
            newName: "Buyer_Info1");

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_Buyer_BuyerId",
            table: "KeyAccount",
            column: "BuyerId",
            principalTable: "Buyer",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_Buyer_BuyerId",
            table: "PriceOffer",
            column: "BuyerId",
            principalTable: "Buyer",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_SaleTeam_Buyer_BuyerId",
            table: "SaleTeam",
            column: "BuyerId",
            principalTable: "Buyer",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_Buyer_BuyerId",
            table: "KeyAccount");

        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_Buyer_BuyerId",
            table: "PriceOffer");

        migrationBuilder.DropForeignKey(
            name: "FK_SaleTeam_Buyer_BuyerId",
            table: "SaleTeam");

        migrationBuilder.RenameColumn(
            name: "BuyerPrice",
            table: "PriceOfferDetail",
            newName: "DistributorPrice");

        migrationBuilder.RenameColumn(
            name: "BuyerId",
            table: "PriceOffer",
            newName: "DistributorId");

        migrationBuilder.RenameIndex(
            name: "IX_PriceOffer_BuyerId",
            table: "PriceOffer",
            newName: "IX_PriceOffer_DistributorId");

        migrationBuilder.RenameColumn(
            name: "Buyer_Info2",
            table: "KeyAccount_Evaluations",
            newName: "Distributor_Info2");

        migrationBuilder.RenameColumn(
            name: "Buyer_Info1",
            table: "KeyAccount_Evaluations",
            newName: "Distributor_Info1");

        migrationBuilder.CreateTable(
            name: "Distributors",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                ContactInfo = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsDeactive = table.Column<bool>(type: "bit", nullable: false),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                PriceColumn = table.Column<short>(type: "smallint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Distributors", x => x.Id);
            });

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_Distributors_BuyerId",
            table: "KeyAccount",
            column: "BuyerId",
            principalTable: "Distributors",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_Buyer_DistributorId",
            table: "PriceOffer",
            column: "DistributorId",
            principalTable: "Buyer",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_SaleTeam_Distributors_BuyerId",
            table: "SaleTeam",
            column: "BuyerId",
            principalTable: "Distributors",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
