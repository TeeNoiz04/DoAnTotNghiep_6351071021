using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_AssetRequest_260324_112930 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgreementNo",
                table: "AssetRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LendingInvoiceNo",
                table: "AssetRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnInvoiceNo",
                table: "AssetRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgreementNo",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "LendingInvoiceNo",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "ReturnInvoiceNo",
                table: "AssetRequest");
        }
    }
}
