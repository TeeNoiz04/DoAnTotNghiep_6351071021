using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_Discussions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Discussion",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                SendTo = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                table.PrimaryKey("PK_Discussion", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Discussion");
    }
}
