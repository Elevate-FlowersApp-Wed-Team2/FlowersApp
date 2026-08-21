using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowersApp.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class seedSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "ArabicTitle", "CategoryId", "CreatedAt", "CreatedBy", "Index", "IsActive", "IsDeleted", "OccasionId", "Title", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("31482c92-c341-476f-bc36-ed98294535be"), "المفضلات لدى العملاء", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7406), "System", 13, true, false, null, "Customer Favorites", 5, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7406), "System" },
                    { new Guid("3329cb70-0d84-452c-afa6-3429ec467058"), "الوافدون الجدد", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7364), "System", 2, true, false, null, "New Arrivals", 1, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7364), "System" },
                    { new Guid("3cb93a91-1f69-4cba-8358-e1287eb93871"), "عروض أعياد الميلاد", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7390), "System", 8, true, false, null, "Birthday Specials", 3, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7390), "System" },
                    { new Guid("506aefd4-44c5-4872-98be-3227f6a7f8f4"), "المنتجات المميزة", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7355), "System", 1, true, false, null, "Featured Products", 1, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7356), "System" },
                    { new Guid("58e50112-7442-4c11-82be-22a5c847dd82"), "تخفيضات الصيف", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7393), "System", 9, true, false, null, "Summer Sale", 4, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7393), "System" },
                    { new Guid("67b3c747-fdeb-4c99-950c-b364c89dce02"), "الأكثر مبيعاً", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7401), "System", 12, true, false, null, "Best Sellers", 5, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7402), "System" },
                    { new Guid("690c9830-06c9-461f-86db-0750563e4570"), "الأكثر رواجاً الآن", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7367), "System", 3, true, false, null, "Trending Now", 1, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7367), "System" },
                    { new Guid("69866bf1-12af-4305-8355-d1078639c8dd"), "مجموعات العطلات", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7379), "System", 7, true, false, null, "Holiday Collections", 3, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7380), "System" },
                    { new Guid("6c0d563c-8ea8-4784-b917-f9cb4cb355e1"), "الفئات الشائعة", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7374), "System", 5, true, false, null, "Popular Categories", 2, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7374), "System" },
                    { new Guid("805a94ae-6e18-4ddc-9f09-aa8a7bdd508c"), "تسوق حسب الفئة", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7370), "System", 4, true, false, null, "Shop by Category", 2, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7370), "System" },
                    { new Guid("971056ae-4e7d-43ea-841f-07e8ee9971c4"), "تخفيضات سريعة", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7398), "System", 11, true, false, null, "Flash Sale", 4, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7399), "System" },
                    { new Guid("ca118084-7bd2-492f-a8cb-4c5eed016d07"), "عرض خاص بعيد الأم", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7396), "System", 10, true, false, null, "Mother's Day Special", 4, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7396), "System" },
                    { new Guid("cd2251fc-22cf-43de-8e7d-5e6329be2d04"), "المناسبات الخاصة", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7377), "System", 6, true, false, null, "Special Occasions", 3, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7377), "System" },
                    { new Guid("f4a5d895-9bcc-4ec5-a568-2ae1403c03d2"), "الأعلى تقييماً", null, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7409), "System", 14, true, false, null, "Top Rated", 5, new DateTime(2026, 8, 21, 14, 27, 0, 619, DateTimeKind.Utc).AddTicks(7409), "System" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("31482c92-c341-476f-bc36-ed98294535be"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("3329cb70-0d84-452c-afa6-3429ec467058"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("3cb93a91-1f69-4cba-8358-e1287eb93871"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("506aefd4-44c5-4872-98be-3227f6a7f8f4"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("58e50112-7442-4c11-82be-22a5c847dd82"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("67b3c747-fdeb-4c99-950c-b364c89dce02"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("690c9830-06c9-461f-86db-0750563e4570"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("69866bf1-12af-4305-8355-d1078639c8dd"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("6c0d563c-8ea8-4784-b917-f9cb4cb355e1"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("805a94ae-6e18-4ddc-9f09-aa8a7bdd508c"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("971056ae-4e7d-43ea-841f-07e8ee9971c4"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("ca118084-7bd2-492f-a8cb-4c5eed016d07"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("cd2251fc-22cf-43de-8e7d-5e6329be2d04"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("f4a5d895-9bcc-4ec5-a568-2ae1403c03d2"));
        }
    }
}
