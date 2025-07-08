using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesSeed4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoomCategories",
                columns: new[] { "Id", "AdultsCapacity", "ChildrenCapacity", "CreatedAt", "Description", "HotelId", "IsDeleted", "Name", "PricePerNight", "RoomType", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1e3d7c4-bb18-4f64-bc0a-01ddb330968c"), 2, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A comfortable standard room for couples.", new Guid("01fbab15-76e6-4e19-02ed-08ddb330968c"), false, "Standard Room", 80.00m, "Luxury", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b2f5f236-c3a2-46d1-bc11-01ddb330968c"), 2, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spacious deluxe room with sea view.", new Guid("01fbab15-76e6-4e19-02ed-08ddb330968c"), false, "Deluxe Room", 120.00m, "Budget", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c3a7e148-f9aa-45d7-91ab-01ddb330968c"), 3, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Family suite for up to 5 guests.", new Guid("01fbab15-76e6-4e19-02ed-08ddb330968c"), false, "Family Suite", 180.00m, "Boutique", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "RoomCategoryId", "RoomNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01fbab15-76e6-4e19-02ed-08ddb330964c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("a1e3d7c4-bb18-4f64-bc0a-01ddb330968c"), "A-22", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("02fbab15-56e6-4e19-02ed-08ddb330964c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("b2f5f236-c3a2-46d1-bc11-01ddb330968c"), "B-22", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "Id",
                keyValue: new Guid("c3a7e148-f9aa-45d7-91ab-01ddb330968c"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("01fbab15-76e6-4e19-02ed-08ddb330964c"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("02fbab15-56e6-4e19-02ed-08ddb330964c"));

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "Id",
                keyValue: new Guid("a1e3d7c4-bb18-4f64-bc0a-01ddb330968c"));

            migrationBuilder.DeleteData(
                table: "RoomCategories",
                keyColumn: "Id",
                keyValue: new Guid("b2f5f236-c3a2-46d1-bc11-01ddb330968c"));
        }
    }
}
