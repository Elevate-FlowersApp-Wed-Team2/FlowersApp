using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlowersApp.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreCoverage : Migration
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
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressStoreAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsUnresolved = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressStoreAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressStoreAssignments_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CoverageAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Geometry = table.Column<Polygon>(type: "geography", nullable: true),
                    CenterLatitude = table.Column<double>(type: "float", nullable: true),
                    CenterLongitude = table.Column<double>(type: "float", nullable: true),
                    RadiusMeters = table.Column<double>(type: "float", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageAreas_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverageCities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoverageAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverageCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverageCities_CoverageAreas_CoverageAreaId",
                        column: x => x.CoverageAreaId,
                        principalTable: "CoverageAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AddressStoreAssignments_AddressId",
                table: "AddressStoreAssignments",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressStoreAssignments_IsUnresolved",
                table: "AddressStoreAssignments",
                column: "IsUnresolved");

            migrationBuilder.CreateIndex(
                name: "IX_AddressStoreAssignments_StoreId",
                table: "AddressStoreAssignments",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAreas_IsActive",
                table: "CoverageAreas",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageAreas_StoreId",
                table: "CoverageAreas",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverageCities_CoverageAreaId",
                table: "CoverageCities",
                column: "CoverageAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_Status",
                table: "Stores",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressStoreAssignments");

            migrationBuilder.DropTable(
                name: "CoverageCities");

            migrationBuilder.DropTable(
                name: "CoverageAreas");

            migrationBuilder.DropTable(
                name: "Stores");

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
