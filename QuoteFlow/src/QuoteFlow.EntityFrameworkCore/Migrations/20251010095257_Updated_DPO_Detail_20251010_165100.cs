using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_DPO_Detail_20251010_165100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DamagedProduct",
                table: "DPODetail",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MEVNSellingInvoiceNo",
                table: "DPODetail",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductSerialNo",
                table: "DPODetail",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DamagedProduct",
                table: "DPODetail");

            migrationBuilder.DropColumn(
                name: "MEVNSellingInvoiceNo",
                table: "DPODetail");

            migrationBuilder.DropColumn(
                name: "ProductSerialNo",
                table: "DPODetail");
        }
    }
}
