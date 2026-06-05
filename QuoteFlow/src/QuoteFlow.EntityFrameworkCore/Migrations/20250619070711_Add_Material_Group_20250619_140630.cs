using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Add_Material_Group_20250619_140630 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MaterialGroups",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Parent = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SortOrder = table.Column<int>(type: "int", nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                MaterialGroupPSI = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                AllowKeyAccount = table.Column<bool>(type: "bit", nullable: false),
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
                table.PrimaryKey("PK_MaterialGroups", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MaterialGroups");
    }
}
