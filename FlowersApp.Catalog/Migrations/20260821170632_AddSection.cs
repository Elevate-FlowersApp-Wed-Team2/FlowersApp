using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowersApp.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class AddSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "IconUrl",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArabicTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OccasionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sections_Occasions_OccasionId",
                        column: x => x.OccasionId,
                        principalTable: "Occasions",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "ArabicTitle", "CategoryId", "CreatedAt", "CreatedBy", "Index", "IsActive", "IsDeleted", "OccasionId", "Title", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("0e496fb9-1c2a-4634-97c1-0286c4f7365f"), "الأكثر مبيعاً", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4957), "System", 12, true, false, null, "Best Sellers", 5, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4957), "System" },
                    { new Guid("156460b5-a0aa-4c94-87df-a918685c6e83"), "تخفيضات سريعة", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4954), "System", 11, true, false, null, "Flash Sale", 4, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4955), "System" },
                    { new Guid("3790fd7d-628c-4f3d-a7c8-79da894fb3fb"), "الفئات الشائعة", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4936), "System", 5, true, false, null, "Popular Categories", 2, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4936), "System" },
                    { new Guid("42492bfc-a28f-473d-8e14-a571b46a77ac"), "المناسبات الخاصة", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4939), "System", 6, true, false, null, "Special Occasions", 3, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4940), "System" },
                    { new Guid("44d8f917-9ddc-45f6-825f-6915fa216723"), "الوافدون الجدد", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4928), "System", 2, true, false, null, "New Arrivals", 1, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4928), "System" },
                    { new Guid("756fd9c6-1c39-42de-9685-aaf90b4fdff8"), "المفضلات لدى العملاء", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4959), "System", 13, true, false, null, "Customer Favorites", 5, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4960), "System" },
                    { new Guid("a3f8274c-0338-4f47-be69-dcf6eb6b56d6"), "مجموعات العطلات", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4942), "System", 7, true, false, null, "Holiday Collections", 3, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4942), "System" },
                    { new Guid("a6c971c2-9b18-4114-8555-a8e2ffce30cd"), "تخفيضات الصيف", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4947), "System", 9, true, false, null, "Summer Sale", 4, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4947), "System" },
                    { new Guid("b456c38c-0f44-4ae0-88ae-7e81fcaca8ee"), "الأكثر رواجاً الآن", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4931), "System", 3, true, false, null, "Trending Now", 1, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4931), "System" },
                    { new Guid("d10f68c3-ebd9-4ed8-a28d-100ae356ba61"), "عروض أعياد الميلاد", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4944), "System", 8, true, false, null, "Birthday Specials", 3, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4945), "System" },
                    { new Guid("d935caad-0f57-4211-8202-6613d9f1fd60"), "المنتجات المميزة", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4910), "System", 1, true, false, null, "Featured Products", 1, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4911), "System" },
                    { new Guid("dcddd700-f441-47e5-ade9-dd6afff560cb"), "عرض خاص بعيد الأم", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4952), "System", 10, true, false, null, "Mother's Day Special", 4, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4952), "System" },
                    { new Guid("f8a9d8b5-5064-4700-ae1f-b285f0620c79"), "تسوق حسب الفئة", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4933), "System", 4, true, false, null, "Shop by Category", 2, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4934), "System" },
                    { new Guid("fb9ea25b-fcd9-455f-ac04-5ca9d5ef6b49"), "الأعلى تقييماً", null, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4962), "System", 14, true, false, null, "Top Rated", 5, new DateTime(2026, 8, 21, 17, 6, 31, 772, DateTimeKind.Utc).AddTicks(4963), "System" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsActive_SortOrder",
                table: "Categories",
                columns: new[] { "IsActive", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CategoryId",
                table: "Sections",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_OccasionId",
                table: "Sections",
                column: "OccasionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Categories_IsActive_SortOrder",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "Categories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "IconUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
