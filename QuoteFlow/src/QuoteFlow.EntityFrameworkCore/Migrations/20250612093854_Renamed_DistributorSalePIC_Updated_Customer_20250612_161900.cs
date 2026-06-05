using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Renamed_DistributorSalePIC_Updated_Customer_20250612_161900 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DistributorSalePIC");

        migrationBuilder.DropColumn(
            name: "CurrentApprovalRouteInstanceId",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "CurrentApprovalStepSequence",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "CurrentSaleRoute",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "DistributorId",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "Email",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "IndustryCode",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "IndustryId",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "JobTitle",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "KeyAccountClassId",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "KeyAccountCode",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "KeyAccountName",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "KeyAccountTypeId",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "LastPODate",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "LastRegisteredProjectCode",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "LocationId",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "MEVNSalePIC",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "MaterialType",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "NationalityId",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "PersonInCharge",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "RegisterName",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "RegisteredKeyAccount",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "RegistrationYear",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "Status",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "TargetEU",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "TargetEndUsers",
            table: "Customer");

        migrationBuilder.RenameColumn(
            name: "KeyAccountShortName",
            table: "Customer",
            newName: "CustomerShortName");

        migrationBuilder.RenameColumn(
            name: "CurrentApproverRoleName",
            table: "Customer",
            newName: "Country");

        migrationBuilder.AlterColumn<string>(
            name: "Website",
            table: "Customer",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Phone",
            table: "Customer",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "CustomerName",
            table: "Customer",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Address",
            table: "Customer",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255,
            oldNullable: true);

        migrationBuilder.CreateTable(
            name: "SaleTeam",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SaleUserName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                SaleFullName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BuyerShortName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                BuyerTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                table.PrimaryKey("PK_SaleTeam", x => x.Id);
                table.ForeignKey(
                    name: "FK_SaleTeam_Distributors_BuyerId",
                    column: x => x.BuyerId,
                    principalTable: "Distributors",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_SaleTeam_SystemCategories_BuyerTypeId",
                    column: x => x.BuyerTypeId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_SaleTeam_SystemCategories_LocationId",
                    column: x => x.LocationId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SaleTeam_BuyerId",
            table: "SaleTeam",
            column: "BuyerId");

        migrationBuilder.CreateIndex(
            name: "IX_SaleTeam_BuyerTypeId",
            table: "SaleTeam",
            column: "BuyerTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_SaleTeam_LocationId",
            table: "SaleTeam",
            column: "LocationId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SaleTeam");

        //migrationBuilder.RenameColumn(
        //    name: "CustomerShortName",
        //    table: "Customer",
        //    newName: "KeyAccountShortName");

        //migrationBuilder.RenameColumn(
        //    name: "Country",
        //    table: "Customer",
        //    newName: "CurrentApproverRoleName");

        //migrationBuilder.AlterColumn<string>(
        //    name: "Website",
        //    table: "Customer",
        //    type: "nvarchar(255)",
        //    maxLength: 255,
        //    nullable: true,
        //    oldClrType: typeof(string),
        //    oldType: "nvarchar(4000)",
        //    oldMaxLength: 4000,
        //    oldNullable: true);

        //migrationBuilder.AlterColumn<string>(
        //    name: "Phone",
        //    table: "Customer",
        //    type: "nvarchar(255)",
        //    maxLength: 255,
        //    nullable: true,
        //    oldClrType: typeof(string),
        //    oldType: "nvarchar(400)",
        //    oldMaxLength: 400,
        //    oldNullable: true);

        //migrationBuilder.AlterColumn<string>(
        //    name: "CustomerName",
        //    table: "Customer",
        //    type: "nvarchar(255)",
        //    maxLength: 255,
        //    nullable: true,
        //    oldClrType: typeof(string),
        //    oldType: "nvarchar(400)",
        //    oldMaxLength: 400);

        //migrationBuilder.AlterColumn<string>(
        //    name: "Address",
        //    table: "Customer",
        //    type: "nvarchar(255)",
        //    maxLength: 255,
        //    nullable: true,
        //    oldClrType: typeof(string),
        //    oldType: "nvarchar(4000)",
        //    oldMaxLength: 4000,
        //    oldNullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "CurrentApprovalRouteInstanceId",
        //    table: "Customer",
        //    type: "uniqueidentifier",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "CurrentApprovalStepSequence",
        //    table: "Customer",
        //    type: "nvarchar(max)",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "CurrentSaleRoute",
        //    table: "Customer",
        //    type: "nvarchar(4000)",
        //    maxLength: 4000,
        //    nullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "DistributorId",
        //    table: "Customer",
        //    type: "uniqueidentifier",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "Email",
        //    table: "Customer",
        //    type: "nvarchar(255)",
        //    maxLength: 255,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "IndustryCode",
        //    table: "Customer",
        //    type: "nvarchar(4000)",
        //    maxLength: 4000,
        //    nullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "IndustryId",
        //    table: "Customer",
        //    type: "uniqueidentifier",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "JobTitle",
        //    table: "Customer",
        //    type: "nvarchar(4000)",
        //    maxLength: 4000,
        //    nullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "KeyAccountClassId",
        //    table: "Customer",
        //    type: "uniqueidentifier",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "KeyAccountCode",
        //    table: "Customer",
        //    type: "nvarchar(255)",
        //    maxLength: 255,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "KeyAccountName",
        //    table: "Customer",
        //    type: "nvarchar(400)",
        //    maxLength: 400,
        //    nullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "KeyAccountTypeId",
        //    table: "Customer",
        //    type: "uniqueidentifier",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "LastPODate",
        //    table: "Customer",
        //    type: "nvarchar(4000)",
        //    maxLength: 4000,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "LastRegisteredProjectCode",
        //    table: "Customer",
        //    type: "nvarchar(50)",
        //    maxLength: 50,
        //    nullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "LocationId",
        //    table: "Customer",
        //    type: "uniqueidentifier",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "MEVNSalePIC",
        //    table: "Customer",
        //    type: "nvarchar(50)",
        //    maxLength: 50,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "MaterialType",
        //    table: "Customer",
        //    type: "nvarchar(50)",
        //    maxLength: 50,
        //    nullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "NationalityId",
        //    table: "Customer",
        //    type: "uniqueidentifier",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "PersonInCharge",
        //    table: "Customer",
        //    type: "nvarchar(4000)",
        //    maxLength: 4000,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "RegisterName",
        //    table: "Customer",
        //    type: "nvarchar(100)",
        //    maxLength: 100,
        //    nullable: true);

        //migrationBuilder.AddColumn<bool>(
        //    name: "RegisteredKeyAccount",
        //    table: "Customer",
        //    type: "bit",
        //    nullable: false,
        //    defaultValue: false);

        //migrationBuilder.AddColumn<int>(
        //    name: "RegistrationYear",
        //    table: "Customer",
        //    type: "int",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "Status",
        //    table: "Customer",
        //    type: "nvarchar(50)",
        //    maxLength: 50,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "TargetEU",
        //    table: "Customer",
        //    type: "nvarchar(4000)",
        //    maxLength: 4000,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "TargetEndUsers",
        //    table: "Customer",
        //    type: "nvarchar(4000)",
        //    maxLength: 4000,
        //    nullable: true);

        migrationBuilder.CreateTable(
            name: "DistributorSalePIC",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DistributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                MaterialTypes = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                SaleFullName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                SaleUserName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DistributorSalePIC", x => x.Id);
                table.ForeignKey(
                    name: "FK_DistributorSalePIC_Distributors_DistributorId",
                    column: x => x.DistributorId,
                    principalTable: "Distributors",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_DistributorSalePIC_SystemCategories_LocationId",
                    column: x => x.LocationId,
                    principalTable: "SystemCategories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DistributorSalePIC_DistributorId",
            table: "DistributorSalePIC",
            column: "DistributorId");

        migrationBuilder.CreateIndex(
            name: "IX_DistributorSalePIC_LocationId",
            table: "DistributorSalePIC",
            column: "LocationId");
    }
}
