using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowersApp.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class EditSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("0e496fb9-1c2a-4634-97c1-0286c4f7365f"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("156460b5-a0aa-4c94-87df-a918685c6e83"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("3790fd7d-628c-4f3d-a7c8-79da894fb3fb"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("42492bfc-a28f-473d-8e14-a571b46a77ac"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("44d8f917-9ddc-45f6-825f-6915fa216723"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("756fd9c6-1c39-42de-9685-aaf90b4fdff8"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("a3f8274c-0338-4f47-be69-dcf6eb6b56d6"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("a6c971c2-9b18-4114-8555-a8e2ffce30cd"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("b456c38c-0f44-4ae0-88ae-7e81fcaca8ee"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("d10f68c3-ebd9-4ed8-a28d-100ae356ba61"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("d935caad-0f57-4211-8202-6613d9f1fd60"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("dcddd700-f441-47e5-ade9-dd6afff560cb"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("f8a9d8b5-5064-4700-ae1f-b285f0620c79"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("fb9ea25b-fcd9-455f-ac04-5ca9d5ef6b49"));

            migrationBuilder.DropColumn(
                name: "ArabicTitle",
                table: "Sections");

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatedBy", "Index", "IsActive", "IsDeleted", "OccasionId", "Title", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("00f62b00-da4a-43d9-8979-fd963db01dee"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8820), "System", 8, true, false, null, "Birthday Specials", 3, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8820), "System" },
                    { new Guid("47afe3b9-f7a6-44fd-9f93-7553124bee1b"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8831), "System", 12, true, false, null, "Best Sellers", 5, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8832), "System" },
                    { new Guid("4dcfa0eb-66d1-464f-baca-d6322f59cab6"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8836), "System", 14, true, false, null, "Top Rated", 5, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8837), "System" },
                    { new Guid("61b984ef-8498-4666-8e4c-0fc7de161eee"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8823), "System", 9, true, false, null, "Summer Sale", 4, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8823), "System" },
                    { new Guid("6234ab21-d8c0-4009-a71b-f305ebb76378"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8751), "System", 4, true, false, null, "Shop by Category", 2, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8752), "System" },
                    { new Guid("6505e5a6-2e80-4020-b682-2bd995949177"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8738), "System", 1, true, false, null, "Featured Products", 1, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8740), "System" },
                    { new Guid("6741db93-9e3b-4bab-a09c-a383ea475da6"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8829), "System", 11, true, false, null, "Flash Sale", 4, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8829), "System" },
                    { new Guid("74ec7f26-d798-4a76-89bf-268f9faeb6cb"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8834), "System", 13, true, false, null, "Customer Favorites", 5, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8834), "System" },
                    { new Guid("9ff9f9e5-515b-4e34-b371-c53486b92e5c"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8754), "System", 5, true, false, null, "Popular Categories", 2, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8754), "System" },
                    { new Guid("b444ddf3-4ae0-4a01-9ea3-cedfd3bba197"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8826), "System", 10, true, false, null, "Mother's Day Special", 4, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8826), "System" },
                    { new Guid("bf73b2a9-d8dd-49f6-88a5-00c5394b238d"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8810), "System", 7, true, false, null, "Holiday Collections", 3, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8810), "System" },
                    { new Guid("d0b082c1-5e07-494b-9211-359061a0fd7f"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8806), "System", 6, true, false, null, "Special Occasions", 3, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8806), "System" },
                    { new Guid("f72a7fd5-0c0c-491a-b40b-a396579ad06a"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8745), "System", 2, true, false, null, "New Arrivals", 1, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8746), "System" },
                    { new Guid("fc11c499-7f4e-4445-ae4f-9e750364de8e"), null, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8748), "System", 3, true, false, null, "Trending Now", 1, new DateTime(2026, 8, 21, 17, 35, 11, 173, DateTimeKind.Utc).AddTicks(8748), "System" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("00f62b00-da4a-43d9-8979-fd963db01dee"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("47afe3b9-f7a6-44fd-9f93-7553124bee1b"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("4dcfa0eb-66d1-464f-baca-d6322f59cab6"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("61b984ef-8498-4666-8e4c-0fc7de161eee"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("6234ab21-d8c0-4009-a71b-f305ebb76378"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("6505e5a6-2e80-4020-b682-2bd995949177"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("6741db93-9e3b-4bab-a09c-a383ea475da6"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("74ec7f26-d798-4a76-89bf-268f9faeb6cb"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("9ff9f9e5-515b-4e34-b371-c53486b92e5c"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("b444ddf3-4ae0-4a01-9ea3-cedfd3bba197"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("bf73b2a9-d8dd-49f6-88a5-00c5394b238d"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("d0b082c1-5e07-494b-9211-359061a0fd7f"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("f72a7fd5-0c0c-491a-b40b-a396579ad06a"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("fc11c499-7f4e-4445-ae4f-9e750364de8e"));

            migrationBuilder.AddColumn<string>(
                name: "ArabicTitle",
                table: "Sections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
        }
    }
}
