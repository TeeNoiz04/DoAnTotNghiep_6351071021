using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_MaterialStockLockStock : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        //migrationBuilder.AddColumn<Guid>(
        //    name: "PSI_Id",
        //    table: "ApprovalRoute",
        //    type: "uniqueidentifier",
        //    nullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "PSI_Id",
        //    table: "ApprovalHistories",
        //    type: "uniqueidentifier",
        //    nullable: true);

        migrationBuilder.CreateTable(
            name: "MaterialStock_LockStock",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GolfaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DPODetail_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                StockCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Qty = table.Column<int>(type: "int", nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ReleasedLock = table.Column<int>(type: "int", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeleterUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                DeleterName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialStock_LockStock", x => x.Id);
            });

        //migrationBuilder.CreateIndex(
        //    name: "IX_ApprovalRoute_PSI_Id",
        //    table: "ApprovalRoute",
        //    column: "PSI_Id");

        //migrationBuilder.CreateIndex(
        //    name: "IX_ApprovalHistories_PSI_Id",
        //    table: "ApprovalHistories",
        //    column: "PSI_Id");

        //migrationBuilder.AddForeignKey(
        //    name: "FK_ApprovalHistories_PSI_PSI_Id",
        //    table: "ApprovalHistories",
        //    column: "PSI_Id",
        //    principalTable: "PSI",
        //    principalColumn: "Id",
        //    onDelete: ReferentialAction.Cascade);

        //migrationBuilder.AddForeignKey(
        //    name: "FK_ApprovalRoute_PSI_PSI_Id",
        //    table: "ApprovalRoute",
        //    column: "PSI_Id",
        //    principalTable: "PSI",
        //    principalColumn: "Id",
        //    onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ApprovalHistories_PSI_PSI_Id",
            table: "ApprovalHistories");

        migrationBuilder.DropForeignKey(
            name: "FK_ApprovalRoute_PSI_PSI_Id",
            table: "ApprovalRoute");

        migrationBuilder.DropTable(
            name: "MaterialStock_LockStock");

        migrationBuilder.DropIndex(
            name: "IX_ApprovalRoute_PSI_Id",
            table: "ApprovalRoute");

        migrationBuilder.DropIndex(
            name: "IX_ApprovalHistories_PSI_Id",
            table: "ApprovalHistories");

        migrationBuilder.DropColumn(
            name: "PSI_Id",
            table: "ApprovalRoute");

        migrationBuilder.DropColumn(
            name: "PSI_Id",
            table: "ApprovalHistories");
    }
}
