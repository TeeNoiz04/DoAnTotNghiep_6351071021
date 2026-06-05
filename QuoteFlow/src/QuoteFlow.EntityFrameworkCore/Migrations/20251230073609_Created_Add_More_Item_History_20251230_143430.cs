using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Created_Add_More_Item_History_20251230_143430 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "CDNo",
            //    table: "StockImport",
            //    type: "nvarchar(4000)",
            //    maxLength: 4000,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100,
            //    oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AddMoreItemHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImportGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Spec1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Spec2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: true),
                    StandardPriceToDist = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StandardAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DistRequestedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequestedDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceToCustomer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceOffer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CometiorBrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetiorModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetiorPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_AddMoreItemHistories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddMoreItemHistories");

            //migrationBuilder.AlterColumn<string>(
            //    name: "CDNo",
            //    table: "StockImport",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(4000)",
            //    oldMaxLength: 4000,
            //    oldNullable: true);
        }
    }
}
