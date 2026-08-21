using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowersApp.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryOccasionConfigurationsAndProductOccasionJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Occasion",
                table: "Occasion");

            migrationBuilder.RenameTable(
                name: "Occasion",
                newName: "Occasions");

            migrationBuilder.RenameIndex(
                name: "IX_Occasion_IsActive_SortOrder",
                table: "Occasions",
                newName: "IX_Occasions_IsActive_SortOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Occasions",
                table: "Occasions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductOccasions",
                columns: table => new
                {
                    OccasionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOccasions", x => new { x.OccasionsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductOccasions_Occasions_OccasionsId",
                        column: x => x.OccasionsId,
                        principalTable: "Occasions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOccasions_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOccasions_ProductsId",
                table: "ProductOccasions",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductOccasions");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Occasions",
                table: "Occasions");

            migrationBuilder.RenameTable(
                name: "Occasions",
                newName: "Occasion");

            migrationBuilder.RenameIndex(
                name: "IX_Occasions_IsActive_SortOrder",
                table: "Occasion",
                newName: "IX_Occasion_IsActive_SortOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Occasion",
                table: "Occasion",
                column: "Id");
        }
    }
}
