using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Approval_History_20251230_161630 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImportGuid",
                table: "ApprovalHistories",
                type: "uniqueidentifier",
                nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "LastModifierUsername",
            //    table: "AddMoreItemHistories",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "LastModifierName",
            //    table: "AddMoreItemHistories",
            //    type: "nvarchar(255)",
            //    maxLength: 255,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "CreatorUsername",
            //    table: "AddMoreItemHistories",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "CreatorName",
            //    table: "AddMoreItemHistories",
            //    type: "nvarchar(255)",
            //    maxLength: 255,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CompetiorPrice",
                table: "AddMoreItemHistories",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportGuid",
                table: "ApprovalHistories");

            //migrationBuilder.AlterColumn<string>(
            //    name: "LastModifierUsername",
            //    table: "AddMoreItemHistories",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "LastModifierName",
            //    table: "AddMoreItemHistories",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(255)",
            //    oldMaxLength: 255,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "CreatorUsername",
            //    table: "AddMoreItemHistories",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "CreatorName",
            //    table: "AddMoreItemHistories",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(255)",
            //    oldMaxLength: 255,
            //    oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompetiorPrice",
                table: "AddMoreItemHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
