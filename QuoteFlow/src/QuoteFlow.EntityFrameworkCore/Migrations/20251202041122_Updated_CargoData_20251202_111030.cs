using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_CargoData_20251202_111030 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_CfgDiscountRatio",
            //    table: "CfgDiscountRatio");

            //migrationBuilder.RenameTable(
            //    name: "CfgDiscountRatio",
            //    newName: "CFG_DiscountRatio");

            //migrationBuilder.AddColumn<string>(
            //    name: "AfterChange",
            //    table: "HistoryTracking",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "BeforeChange",
            //    table: "HistoryTracking",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "CargoData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "CargoData",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierCode",
                table: "CargoData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_CFG_DiscountRatio",
            //    table: "CFG_DiscountRatio",
            //    column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_CFG_DiscountRatio",
            //    table: "CFG_DiscountRatio");

            //migrationBuilder.DropColumn(
            //    name: "AfterChange",
            //    table: "HistoryTracking");

            //migrationBuilder.DropColumn(
            //    name: "BeforeChange",
            //    table: "HistoryTracking");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "CargoData");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "CargoData");

            migrationBuilder.DropColumn(
                name: "SupplierCode",
                table: "CargoData");

            //migrationBuilder.RenameTable(
            //    name: "CFG_DiscountRatio",
            //    newName: "CfgDiscountRatio");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_CfgDiscountRatio",
            //    table: "CfgDiscountRatio",
            //    column: "Id");
        }
    }
}
