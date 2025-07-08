using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedEntities2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: new Guid("85e91235-6799-4e63-a35b-920601e1a9dc"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("85e91235-6799-4e62-a36b-920601e1a9db"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("85e91235-6799-4e62-a37b-920601e1a9dc"));

            migrationBuilder.DeleteData(
                table: "RoomInfos",
                keyColumn: "Id",
                keyValue: new Guid("85e91235-6799-4e63-a35b-920601e1a9db"));

            migrationBuilder.DeleteData(
                table: "RoomInfos",
                keyColumn: "Id",
                keyValue: new Guid("85e91235-6799-4e64-a35b-920601e1a9dc"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoomInfos",
                columns: new[] { "Id", "AdultsCapacity", "ChildrenCapacity", "CreatedAt", "Description", "HotelId", "IsDeleted", "Name", "PricePerNight", "RoomType", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("85e91235-6799-4e63-a35b-920601e1a9db"), 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spacious deluxe room with king bed.", new Guid("85e91235-6799-4e62-a35b-920601e1a9db"), false, "Deluxe Room", 150m, "Luxury", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("85e91235-6799-4e64-a35b-920601e1a9dc"), 2, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comfortable room with sea view.", new Guid("85e91235-6799-4e62-a35b-920601e1a9dc"), false, "Sea View Room", 200m, "Budget", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "DiscountPercentage", "EndDate", "RoomInfoId", "StartDate" },
                values: new object[] { new Guid("85e91235-6799-4e63-a35b-920601e1a9dc"), 15m, new DateTime(2025, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("85e91235-6799-4e63-a35b-920601e1a9db"), new DateTime(2025, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "RoomInfoId", "RoomNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("85e91235-6799-4e62-a36b-920601e1a9db"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("85e91235-6799-4e63-a35b-920601e1a9db"), "101", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("85e91235-6799-4e62-a37b-920601e1a9dc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new Guid("85e91235-6799-4e64-a35b-920601e1a9dc"), "202", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
