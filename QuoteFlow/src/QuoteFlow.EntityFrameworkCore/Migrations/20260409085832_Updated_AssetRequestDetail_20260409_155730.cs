using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_AssetRequestDetail_20260409_155730 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AF_PIC",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Counted_Quantity",
                table: "AssetRequestDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FAI_PIC",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FAP_PIC",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IA_PIC",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Variance",
                table: "AssetRequestDetail",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AF_PIC",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Counted_Quantity",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "FAI_PIC",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "FAP_PIC",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "IA_PIC",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Variance",
                table: "AssetRequestDetail");
        }
    }
}
