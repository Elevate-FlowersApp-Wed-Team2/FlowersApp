using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowersApp.Auth.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editDocEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Applications_DriverApplicationId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_DriverApplicationId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DriverApplicationId",
                table: "Documents");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ApplicationId",
                table: "Documents",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Applications_ApplicationId",
                table: "Documents",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Applications_ApplicationId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ApplicationId",
                table: "Documents");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationId",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "DriverApplicationId",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DriverApplicationId",
                table: "Documents",
                column: "DriverApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Applications_DriverApplicationId",
                table: "Documents",
                column: "DriverApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
