using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_CustomerPICs_20250624_101400 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Customer_PIC",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                KeyAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PICName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                PIC_Phone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                PIC_Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                PIC_JobTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                Remark = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                table.PrimaryKey("PK_Customer_PIC", x => x.Id);
                table.ForeignKey(
                    name: "FK_Customer_PIC_KeyAccount_KeyAccountId",
                    column: x => x.KeyAccountId,
                    principalTable: "KeyAccount",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Customer_PIC_KeyAccountId",
            table: "Customer_PIC",
            column: "KeyAccountId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Customer_PIC");
    }
}
