using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_DPO_20251024_140000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "DPO",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "DPO");
        }
    }
}
