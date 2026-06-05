using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_DPO_20251028_173200 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentApprovalRouteInstanceId",
                table: "DPO",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentApprovalStepSequence",
                table: "DPO",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentApproverRoleCode",
                table: "DPO",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentApproverRoleName",
                table: "DPO",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gkr_LinkedNote",
                table: "DPO",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GkrId",
                table: "ApprovalRoute",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRoute_GkrId",
                table: "ApprovalRoute",
                column: "GkrId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalRoute_DPO_GkrId",
                table: "ApprovalRoute",
                column: "GkrId",
                principalTable: "DPO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalRoute_DPO_GkrId",
                table: "ApprovalRoute");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalRoute_GkrId",
                table: "ApprovalRoute");

            migrationBuilder.DropColumn(
                name: "CurrentApprovalRouteInstanceId",
                table: "DPO");

            migrationBuilder.DropColumn(
                name: "CurrentApprovalStepSequence",
                table: "DPO");

            migrationBuilder.DropColumn(
                name: "CurrentApproverRoleCode",
                table: "DPO");

            migrationBuilder.DropColumn(
                name: "CurrentApproverRoleName",
                table: "DPO");

            migrationBuilder.DropColumn(
                name: "Gkr_LinkedNote",
                table: "DPO");

            migrationBuilder.DropColumn(
                name: "GkrId",
                table: "ApprovalRoute");
        }
    }
}
