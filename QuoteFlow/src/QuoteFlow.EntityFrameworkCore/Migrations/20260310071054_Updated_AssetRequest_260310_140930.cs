using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_AssetRequest_260310_140930 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Assets");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AssetRequest",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "AssetRequest");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Assets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
