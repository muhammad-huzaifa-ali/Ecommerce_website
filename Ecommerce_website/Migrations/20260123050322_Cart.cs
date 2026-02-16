using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_website.Migrations
{
    /// <inheritdoc />
    public partial class Cart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "Tbl_Cart");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Tbl_Cart",
                newName: "product_status");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "Tbl_Cart",
                newName: "product_quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "product_status",
                table: "Tbl_Cart",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "product_quantity",
                table: "Tbl_Cart",
                newName: "quantity");

            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "Tbl_Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
