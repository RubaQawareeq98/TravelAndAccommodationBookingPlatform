using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesSeed3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("01fbab16-76e6-4e19-02ed-08ddb330969c"));

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Country", "CreatedAt", "IsDeleted", "Name", "PostalCode", "ThumbnailUrl", "UpdatedAt" },
                values: new object[] { new Guid("01fbab15-76e6-4e19-02ed-08ddb330969c"), "Palestine", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Nablus", "4104", null, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Email", "FirstName", "IsDeleted", "LastName", "PhoneNumber" },
                values: new object[] { new Guid("01fbab15-76e6-4e19-02ed-08ddb330969d"), "john.doe@gmail.com", "John", false, "Doe", "08888888888" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00fbab16-76e6-4e19-02ed-08ddb330969b"),
                column: "Password",
                value: "$2a$11$8e408FqZzbA4f50erX7K4.T1ZT5KIgS/Fd/Tx5HCQDanSaeeR/vSq");

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "CityId", "CreatedAt", "Description", "Email", "HotelType", "IsDeleted", "Latitude", "Longitude", "Name", "OwnerId", "PhoneNumber", "StarRating", "ThumbnailUrl", "TotalRooms", "UpdatedAt" },
                values: new object[] { new Guid("01fbab15-76e6-4e19-02ed-08ddb330968c"), new Guid("01fbab15-76e6-4e19-02ed-08ddb330969c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Hotel", null, "Business", false, 85.0, -40.0, "Business Hotel", new Guid("01fbab15-76e6-4e19-02ed-08ddb330969d"), "+35987654321", 4, null, 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: new Guid("01fbab15-76e6-4e19-02ed-08ddb330968c"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("01fbab15-76e6-4e19-02ed-08ddb330969c"));

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: new Guid("01fbab15-76e6-4e19-02ed-08ddb330969d"));

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Country", "CreatedAt", "IsDeleted", "Name", "PostalCode", "ThumbnailUrl", "UpdatedAt" },
                values: new object[] { new Guid("01fbab16-76e6-4e19-02ed-08ddb330969c"), "Palestine", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Nablus", "4104", null, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00fbab16-76e6-4e19-02ed-08ddb330969b"),
                column: "Password",
                value: "$2a$11$A1uCUov6PzW2KNc4uRQ9t.AUSCrqKk2bQfI9zpAKrGChUybJ.E1pC");
        }
    }
}
