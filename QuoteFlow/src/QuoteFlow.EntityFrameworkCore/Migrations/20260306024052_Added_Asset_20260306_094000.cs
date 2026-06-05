using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Added_Asset_20260306_094000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PIC_Src = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Lending_CompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Lending_ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentApprovalRouteInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentApprovalStepSequence = table.Column<int>(type: "int", nullable: true),
                    CurrentApproverRoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrentApproverRoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifierUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetRequestDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WarehouseSrcId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehouseSrcName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WarehouseDestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehouseDestName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PICSrc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PICDesc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifierUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRequestDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AssetClass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AssetType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehouseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SalePIC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CodeMain = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodeSub = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodeMain_AF = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodeSub_AF = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumberOfComponent = table.Column<int>(type: "int", nullable: true),
                    POR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GIV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaterialCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Section = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LendingInformation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifierUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetRequest");

            migrationBuilder.DropTable(
                name: "AssetRequestDetail");

            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}
