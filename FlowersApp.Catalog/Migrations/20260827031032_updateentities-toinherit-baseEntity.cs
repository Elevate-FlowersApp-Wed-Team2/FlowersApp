using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowersApp.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class updateentitiestoinheritbaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("057c56e1-0e32-40e3-93a6-9a1d7a8c99c1"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("079cab51-5892-473a-962a-29bd57117349"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("25400259-cdae-4714-81bf-f09944aee35f"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("41e3ee67-db65-4ef9-8560-e1454e81f540"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("4a7f9374-9b58-4484-a55e-7ff9d83d0f82"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("5ba4d655-6b30-464a-93e9-832b376432f0"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("5eb0555e-3df8-4a94-ac50-05f1aa625a3f"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("69f7eb7d-ad06-424f-bd41-1e5d68453014"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("9d30ad39-3ed8-43bb-bdd1-e4cc9024520d"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("c26a5fc6-9636-4a81-86f1-1628ca3a84db"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("c8c731fe-ea12-4c83-8659-c53bfc7933e7"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("e4f04fb8-458b-4751-bb31-d2ad259d5a01"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("e9ea6c3b-eac1-4ff2-9105-173ae3729ca8"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("fc52ed55-1cb6-4ca6-85d9-c406844aee60"));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Stores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CoverageCities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoverageCities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoverageCities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CoverageCities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoverageCities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoverageAreas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoverageAreas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CoverageAreas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoverageAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AddressStoreAssignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AddressStoreAssignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AddressStoreAssignments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AddressStoreAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AddressStoreAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatedBy", "Index", "IsActive", "IsDeleted", "OccasionId", "Title", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("19a112cd-4288-47f4-81ef-1285810fd6e3"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6020), "System", 6, true, false, null, "Special Occasions", 3, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6021), "System" },
                    { new Guid("212db674-6b60-4349-9737-f43c880b7ab0"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6026), "System", 7, true, false, null, "Holiday Collections", 3, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6027), "System" },
                    { new Guid("338596ea-909c-4d4d-9027-ffd554f062d2"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6058), "System", 11, true, false, null, "Flash Sale", 4, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6058), "System" },
                    { new Guid("3f5e4b8c-77be-494d-a6e2-b61a3d2f8501"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6064), "System", 12, true, false, null, "Best Sellers", 5, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6065), "System" },
                    { new Guid("40c37e1c-259b-428e-9dd0-03a190d75811"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6093), "System", 14, true, false, null, "Top Rated", 5, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6094), "System" },
                    { new Guid("4b575dc9-16a8-4836-be8c-57d15b2d91dd"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(5970), "System", 2, true, false, null, "New Arrivals", 1, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(5971), "System" },
                    { new Guid("69a0135a-2d71-441b-a58b-56d3f01664fa"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6000), "System", 3, true, false, null, "Trending Now", 1, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6000), "System" },
                    { new Guid("9fac2a56-702e-4b93-9535-92816f22e0aa"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6033), "System", 8, true, false, null, "Birthday Specials", 3, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6034), "System" },
                    { new Guid("b1170f83-764a-47dd-99ba-6c2e1a7d6a23"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6046), "System", 10, true, false, null, "Mother's Day Special", 4, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6047), "System" },
                    { new Guid("bab9f7e3-b1ac-4e95-8d04-9f4376633080"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6070), "System", 13, true, false, null, "Customer Favorites", 5, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6071), "System" },
                    { new Guid("bf193a81-68c1-4ef1-977e-656b46131e80"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6014), "System", 5, true, false, null, "Popular Categories", 2, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6014), "System" },
                    { new Guid("bf88c20e-1167-45de-8353-831fd1ad29f0"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(5950), "System", 1, true, false, null, "Featured Products", 1, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(5955), "System" },
                    { new Guid("c822a9ca-f0a6-4d0d-87a1-e096195510f4"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6007), "System", 4, true, false, null, "Shop by Category", 2, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6007), "System" },
                    { new Guid("e6b0604a-11bb-48e4-973b-c8b131d335a4"), null, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6039), "System", 9, true, false, null, "Summer Sale", 4, new DateTime(2026, 8, 27, 3, 10, 29, 855, DateTimeKind.Utc).AddTicks(6040), "System" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("19a112cd-4288-47f4-81ef-1285810fd6e3"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("212db674-6b60-4349-9737-f43c880b7ab0"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("338596ea-909c-4d4d-9027-ffd554f062d2"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("3f5e4b8c-77be-494d-a6e2-b61a3d2f8501"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("40c37e1c-259b-428e-9dd0-03a190d75811"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("4b575dc9-16a8-4836-be8c-57d15b2d91dd"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("69a0135a-2d71-441b-a58b-56d3f01664fa"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("9fac2a56-702e-4b93-9535-92816f22e0aa"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("b1170f83-764a-47dd-99ba-6c2e1a7d6a23"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("bab9f7e3-b1ac-4e95-8d04-9f4376633080"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("bf193a81-68c1-4ef1-977e-656b46131e80"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("bf88c20e-1167-45de-8353-831fd1ad29f0"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("c822a9ca-f0a6-4d0d-87a1-e096195510f4"));

            migrationBuilder.DeleteData(
                table: "Sections",
                keyColumn: "Id",
                keyValue: new Guid("e6b0604a-11bb-48e4-973b-c8b131d335a4"));

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CoverageCities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoverageCities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoverageCities");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CoverageCities");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoverageCities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoverageAreas");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoverageAreas");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CoverageAreas");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoverageAreas");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AddressStoreAssignments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AddressStoreAssignments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AddressStoreAssignments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AddressStoreAssignments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AddressStoreAssignments");

            migrationBuilder.InsertData(
                table: "Sections",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatedBy", "Index", "IsActive", "IsDeleted", "OccasionId", "Title", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("057c56e1-0e32-40e3-93a6-9a1d7a8c99c1"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5905), "System", 6, true, false, null, "Special Occasions", 3, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5906), "System" },
                    { new Guid("079cab51-5892-473a-962a-29bd57117349"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5935), "System", 11, true, false, null, "Flash Sale", 4, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5936), "System" },
                    { new Guid("25400259-cdae-4714-81bf-f09944aee35f"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5892), "System", 2, true, false, null, "New Arrivals", 1, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5893), "System" },
                    { new Guid("41e3ee67-db65-4ef9-8560-e1454e81f540"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5925), "System", 9, true, false, null, "Summer Sale", 4, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5925), "System" },
                    { new Guid("4a7f9374-9b58-4484-a55e-7ff9d83d0f82"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5928), "System", 10, true, false, null, "Mother's Day Special", 4, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5928), "System" },
                    { new Guid("5ba4d655-6b30-464a-93e9-832b376432f0"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5945), "System", 14, true, false, null, "Top Rated", 5, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5945), "System" },
                    { new Guid("5eb0555e-3df8-4a94-ac50-05f1aa625a3f"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5882), "System", 1, true, false, null, "Featured Products", 1, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5885), "System" },
                    { new Guid("69f7eb7d-ad06-424f-bd41-1e5d68453014"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5902), "System", 5, true, false, null, "Popular Categories", 2, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5902), "System" },
                    { new Guid("9d30ad39-3ed8-43bb-bdd1-e4cc9024520d"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5899), "System", 4, true, false, null, "Shop by Category", 2, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5899), "System" },
                    { new Guid("c26a5fc6-9636-4a81-86f1-1628ca3a84db"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5909), "System", 7, true, false, null, "Holiday Collections", 3, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5909), "System" },
                    { new Guid("c8c731fe-ea12-4c83-8659-c53bfc7933e7"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5921), "System", 8, true, false, null, "Birthday Specials", 3, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5922), "System" },
                    { new Guid("e4f04fb8-458b-4751-bb31-d2ad259d5a01"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5896), "System", 3, true, false, null, "Trending Now", 1, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5896), "System" },
                    { new Guid("e9ea6c3b-eac1-4ff2-9105-173ae3729ca8"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5939), "System", 12, true, false, null, "Best Sellers", 5, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5939), "System" },
                    { new Guid("fc52ed55-1cb6-4ca6-85d9-c406844aee60"), null, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5942), "System", 13, true, false, null, "Customer Favorites", 5, new DateTime(2026, 8, 27, 0, 15, 54, 300, DateTimeKind.Utc).AddTicks(5942), "System" }
                });
        }
    }
}
