using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_website.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "customer_image",
                table: "Tbl_Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Product_category_id",
                table: "Tbl_Product",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Product_Tbl_Category_category_id",
                table: "Tbl_Product",
                column: "category_id",
                principalTable: "Tbl_Category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Product_Tbl_Category_category_id",
                table: "Tbl_Product");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Product_category_id",
                table: "Tbl_Product");

            migrationBuilder.AlterColumn<string>(
                name: "customer_image",
                table: "Tbl_Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
