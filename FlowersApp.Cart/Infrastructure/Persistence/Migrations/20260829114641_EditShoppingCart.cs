using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowersApp.Cart.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "ShoppingCarts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "ShoppingCarts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "ShoppingCarts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "ShoppingCartItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountedPrice",
                table: "ShoppingCartItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ShoppingCartItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ShoppingCartItems");
        }
    }
}
