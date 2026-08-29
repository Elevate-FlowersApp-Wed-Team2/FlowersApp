using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowersApp.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class SeedGovernoratesAndCities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("1cf0812e-0b42-4ce7-b052-1a3b3ce9aca8"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("24ba8588-8851-4279-b595-2e7bf535365d"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("39e4e8b0-bb2d-4979-b019-5e4cd0e38dda"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("51818fd0-5ec5-43a1-9d03-9a53fc09f4c0"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("5b8ae00f-9a29-4f6e-a898-2f41821263af"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("95eaf337-4f14-446d-b01d-9e4c2e8d9e9b"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("b394f359-8703-4884-8493-2e0d9a1f1fa8"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("ba192ef3-4de8-4cbb-86c8-61ccdc498158"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("cf2f2830-9381-4fa8-8c35-d9facf58901e"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("d174b909-6f35-4f1b-ab42-f09c28a64112"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("e05dc921-8aad-414d-b149-6c7bcc7bc65e"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("e8730c7f-c2de-4574-a39c-a919a220c8d6"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("fac654cc-fc1e-44c0-a5fa-8a10cc435d9c"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("fdd1c93f-58c5-4194-ad6c-77c3d59614a5"));

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatedBy", "Index", "IsActive", "IsDeleted", "OccasionId", "Title", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("0e8f945d-cf3a-46fd-9d9f-ee62a33971c9"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(722), "System", 11, true, false, null, "Flash Sale", 4, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(724), "System" },
                    { new Guid("0fac46f8-7951-4c28-9f0e-1322b27b2ad1"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(731), "System", 12, true, false, null, "Best Sellers", 5, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(733), "System" },
                    { new Guid("2886f6fe-4abb-4ac4-9aa5-d8969d4fec96"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(713), "System", 10, true, false, null, "Mother's Day Special", 4, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(714), "System" },
                    { new Guid("483cfccd-4131-4e15-9f50-157230853260"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(610), "System", 2, true, false, null, "New Arrivals", 1, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(611), "System" },
                    { new Guid("62a8af8c-f009-4be4-84cb-b6d8eeaae649"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(639), "System", 5, true, false, null, "Popular Categories", 2, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(641), "System" },
                    { new Guid("81ba06b3-b6e3-466e-bf2e-373c0f3deb4c"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(740), "System", 13, true, false, null, "Customer Favorites", 5, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(742), "System" },
                    { new Guid("8c2b96fa-41c1-4d5e-8275-0715e7bb4faa"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(705), "System", 9, true, false, null, "Summer Sale", 4, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(706), "System" },
                    { new Guid("a6fc16aa-5627-4727-8126-2b8ab595d1f3"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(630), "System", 4, true, false, null, "Shop by Category", 2, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(631), "System" },
                    { new Guid("c32d3189-eb5b-4c14-ba51-2f1dcae1e64a"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(676), "System", 6, true, false, null, "Special Occasions", 3, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(677), "System" },
                    { new Guid("c90f8fe1-658c-490f-a6a1-d17b71a14564"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(621), "System", 3, true, false, null, "Trending Now", 1, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(623), "System" },
                    { new Guid("db6cc5a4-0f0f-4cc4-966b-a96425a63d8e"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(693), "System", 8, true, false, null, "Birthday Specials", 3, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(695), "System" },
                    { new Guid("e036a787-3a76-4aaf-a5e8-5ace76fbf88b"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(759), "System", 14, true, false, null, "Top Rated", 5, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(760), "System" },
                    { new Guid("e22370ab-ec1f-4634-aacd-a9a6ecc1d217"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(685), "System", 7, true, false, null, "Holiday Collections", 3, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(686), "System" },
                    { new Guid("f003e488-a147-4d0e-a2c1-b67a4da1386b"), null, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(585), "System", 1, true, false, null, "Featured Products", 1, new DateTime(2026, 8, 29, 4, 21, 35, 450, DateTimeKind.Utc).AddTicks(595), "System" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_GovernorateId",
                table: "Cities",
                column: "GovernorateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("0e8f945d-cf3a-46fd-9d9f-ee62a33971c9"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("0fac46f8-7951-4c28-9f0e-1322b27b2ad1"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("2886f6fe-4abb-4ac4-9aa5-d8969d4fec96"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("483cfccd-4131-4e15-9f50-157230853260"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("62a8af8c-f009-4be4-84cb-b6d8eeaae649"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("81ba06b3-b6e3-466e-bf2e-373c0f3deb4c"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("8c2b96fa-41c1-4d5e-8275-0715e7bb4faa"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("a6fc16aa-5627-4727-8126-2b8ab595d1f3"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("c32d3189-eb5b-4c14-ba51-2f1dcae1e64a"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("c90f8fe1-658c-490f-a6a1-d17b71a14564"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("db6cc5a4-0f0f-4cc4-966b-a96425a63d8e"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("e036a787-3a76-4aaf-a5e8-5ace76fbf88b"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("e22370ab-ec1f-4634-aacd-a9a6ecc1d217"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("f003e488-a147-4d0e-a2c1-b67a4da1386b"));

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatedBy", "Index", "IsActive", "IsDeleted", "OccasionId", "Title", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("1cf0812e-0b42-4ce7-b052-1a3b3ce9aca8"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7156), "System", 2, true, false, null, "New Arrivals", 1, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7157), "System" },
                    { new Guid("24ba8588-8851-4279-b595-2e7bf535365d"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7138), "System", 1, true, false, null, "Featured Products", 1, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7141), "System" },
                    { new Guid("39e4e8b0-bb2d-4979-b019-5e4cd0e38dda"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7176), "System", 8, true, false, null, "Birthday Specials", 3, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7176), "System" },
                    { new Guid("51818fd0-5ec5-43a1-9d03-9a53fc09f4c0"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7294), "System", 13, true, false, null, "Customer Favorites", 5, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7294), "System" },
                    { new Guid("5b8ae00f-9a29-4f6e-a898-2f41821263af"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7166), "System", 5, true, false, null, "Popular Categories", 2, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7166), "System" },
                    { new Guid("95eaf337-4f14-446d-b01d-9e4c2e8d9e9b"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7163), "System", 4, true, false, null, "Shop by Category", 2, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7163), "System" },
                    { new Guid("b394f359-8703-4884-8493-2e0d9a1f1fa8"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7169), "System", 6, true, false, null, "Special Occasions", 3, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7170), "System" },
                    { new Guid("ba192ef3-4de8-4cbb-86c8-61ccdc498158"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7188), "System", 11, true, false, null, "Flash Sale", 4, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7188), "System" },
                    { new Guid("cf2f2830-9381-4fa8-8c35-d9facf58901e"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7297), "System", 14, true, false, null, "Top Rated", 5, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7297), "System" },
                    { new Guid("d174b909-6f35-4f1b-ab42-f09c28a64112"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7290), "System", 12, true, false, null, "Best Sellers", 5, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7291), "System" },
                    { new Guid("e05dc921-8aad-414d-b149-6c7bcc7bc65e"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7185), "System", 10, true, false, null, "Mother's Day Special", 4, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7185), "System" },
                    { new Guid("e8730c7f-c2de-4574-a39c-a919a220c8d6"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7172), "System", 7, true, false, null, "Holiday Collections", 3, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7172), "System" },
                    { new Guid("fac654cc-fc1e-44c0-a5fa-8a10cc435d9c"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7179), "System", 9, true, false, null, "Summer Sale", 4, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7179), "System" },
                    { new Guid("fdd1c93f-58c5-4194-ad6c-77c3d59614a5"), null, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7160), "System", 3, true, false, null, "Trending Now", 1, new DateTime(2026, 8, 24, 21, 48, 18, 842, DateTimeKind.Utc).AddTicks(7160), "System" }
                });
        }
    }
}
