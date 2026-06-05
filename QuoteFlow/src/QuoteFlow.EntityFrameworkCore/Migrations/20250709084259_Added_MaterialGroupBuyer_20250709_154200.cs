using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_MaterialGroupBuyer_20250709_154200 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {

        migrationBuilder.CreateTable(
            name: "MaterialGroupBuyer",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MaterialGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                MaterialGroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                BuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BuyerShortName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialGroupBuyer", x => x.Id);
                table.ForeignKey(
                    name: "FK_MaterialGroupBuyer_Buyer_BuyerId",
                    column: x => x.BuyerId,
                    principalTable: "Buyer",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MaterialGroupBuyer_MaterialGroups_MaterialGroupId",
                    column: x => x.MaterialGroupId,
                    principalTable: "MaterialGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MaterialGroupBuyer_BuyerId",
            table: "MaterialGroupBuyer",
            column: "BuyerId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialGroupBuyer_MaterialGroupId",
            table: "MaterialGroupBuyer",
            column: "MaterialGroupId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MaterialGroupBuyer");


    }
}
