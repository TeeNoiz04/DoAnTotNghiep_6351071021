using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SpecialInputPrice_20250825_111200 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SpecialInputPrice_SupplierBUId",
                table: "SpecialInputPrice",
                column: "SupplierBUId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialInputPrice_SupplierId",
                table: "SpecialInputPrice",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialInputPrice_Supplier_BU_SupplierBUId",
                table: "SpecialInputPrice",
                column: "SupplierBUId",
                principalTable: "Supplier_BU",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialInputPrice_Supplier_SupplierId",
                table: "SpecialInputPrice",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialInputPrice_Supplier_BU_SupplierBUId",
                table: "SpecialInputPrice");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialInputPrice_Supplier_SupplierId",
                table: "SpecialInputPrice");

            migrationBuilder.DropIndex(
                name: "IX_SpecialInputPrice_SupplierBUId",
                table: "SpecialInputPrice");

            migrationBuilder.DropIndex(
                name: "IX_SpecialInputPrice_SupplierId",
                table: "SpecialInputPrice");
        }
    }
}
