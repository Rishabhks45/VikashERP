using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikashERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSellingUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellingUnit",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellingUnit",
                table: "Products");
        }
    }
}
