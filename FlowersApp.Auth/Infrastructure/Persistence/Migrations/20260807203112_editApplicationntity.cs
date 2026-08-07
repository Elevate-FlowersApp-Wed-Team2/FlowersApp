using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowersApp.Auth.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editApplicationntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FcmToken",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FcmToken",
                table: "Applications");
        }
    }
}
