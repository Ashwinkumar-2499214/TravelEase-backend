using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelEase.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationAndEmailChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetEmail",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetEmail",
                table: "Notifications");
        }
    }
}
