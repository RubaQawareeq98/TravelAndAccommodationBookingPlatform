using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndexesSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Discounts_EndDate",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_StartDate",
                table: "Discounts");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_IsDeleted",
                table: "Rooms",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RoomCategories_IsDeleted",
                table: "RoomCategories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_IsDeleted",
                table: "Hotels",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_StartDate_EndDate",
                table: "Discounts",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_IsDeleted",
                table: "Cities",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_IsDeleted",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_RoomCategories_IsDeleted",
                table: "RoomCategories");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_IsDeleted",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_StartDate_EndDate",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Cities_IsDeleted",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_EndDate",
                table: "Discounts",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_StartDate",
                table: "Discounts",
                column: "StartDate");
        }
    }
}
