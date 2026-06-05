using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedStockImport_20260121_092130 : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "ConfirmNote",
                table: "StockImport",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "ModelName",
            //    table: "SaleOrdersSAPImport",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(50)",
            //    oldMaxLength: 50,
            //    oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmNote",
                table: "StockImport");

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

            //migrationBuilder.AlterColumn<string>(
            //    name: "ModelName",
            //    table: "SaleOrdersSAPImport",
            //    type: "nvarchar(50)",
            //    maxLength: 50,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100,
            //    oldNullable: true);
        }
    }
}
