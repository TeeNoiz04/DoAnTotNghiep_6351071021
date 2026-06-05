using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_KeyAccount_20250611_173000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_KeyAccount_DistributorId",
            table: "KeyAccount",
            column: "DistributorId");

        migrationBuilder.CreateIndex(
            name: "IX_KeyAccount_KeyAccountClassId",
            table: "KeyAccount",
            column: "KeyAccountClassId");

        migrationBuilder.CreateIndex(
            name: "IX_KeyAccount_KeyAccountTypeId",
            table: "KeyAccount",
            column: "KeyAccountTypeId");

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_Distributors_DistributorId",
            table: "KeyAccount",
            column: "DistributorId",
            principalTable: "Distributors",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_SystemCategories_KeyAccountClassId",
            table: "KeyAccount",
            column: "KeyAccountClassId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_SystemCategories_KeyAccountTypeId",
            table: "KeyAccount",
            column: "KeyAccountTypeId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_Distributors_DistributorId",
            table: "KeyAccount");

        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_SystemCategories_KeyAccountClassId",
            table: "KeyAccount");

        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_SystemCategories_KeyAccountTypeId",
            table: "KeyAccount");

        migrationBuilder.DropIndex(
            name: "IX_KeyAccount_DistributorId",
            table: "KeyAccount");

        migrationBuilder.DropIndex(
            name: "IX_KeyAccount_KeyAccountClassId",
            table: "KeyAccount");

        migrationBuilder.DropIndex(
            name: "IX_KeyAccount_KeyAccountTypeId",
            table: "KeyAccount");
    }
}
