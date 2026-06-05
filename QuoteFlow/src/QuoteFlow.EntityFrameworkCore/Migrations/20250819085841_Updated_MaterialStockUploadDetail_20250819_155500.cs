using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_MaterialStockUploadDetail_20250819_155500 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "StorageDesc_Id",
            table: "MaterialStockUploadDetail",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "StorageSrc_Id",
            table: "MaterialStockUploadDetail",
            type: "uniqueidentifier",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "StorageDesc_Id",
            table: "MaterialStockUploadDetail");

        migrationBuilder.DropColumn(
            name: "StorageSrc_Id",
            table: "MaterialStockUploadDetail");
    }
}
