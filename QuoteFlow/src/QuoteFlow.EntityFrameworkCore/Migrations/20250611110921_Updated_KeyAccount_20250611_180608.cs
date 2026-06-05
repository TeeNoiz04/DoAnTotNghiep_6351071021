using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_KeyAccount_20250611_180608 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_Distributors_DistributorId",
            table: "KeyAccount");

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_Buyer_DistributorId",
            table: "KeyAccount",
            column: "DistributorId",
            principalTable: "Buyer",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_Buyer_DistributorId",
            table: "KeyAccount");

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_Distributors_DistributorId",
            table: "KeyAccount",
            column: "DistributorId",
            principalTable: "Distributors",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
