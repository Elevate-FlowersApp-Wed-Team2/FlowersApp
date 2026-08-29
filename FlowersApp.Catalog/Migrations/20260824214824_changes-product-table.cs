using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowersApp.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class changesproducttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrls",
                table: "Products",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Includes",
                table: "Products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Includes",
                table: "Products");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
