using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GalleryImages_Hotels_HotelId",
                table: "GalleryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_GalleryImages_RoomCategories_RoomCategoryId",
                table: "GalleryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_GalleryImages_Rooms_RoomId",
                table: "GalleryImages");

            migrationBuilder.DropIndex(
                name: "IX_GalleryImages_HotelId",
                table: "GalleryImages");

            migrationBuilder.DropIndex(
                name: "IX_GalleryImages_RoomCategoryId",
                table: "GalleryImages");

            migrationBuilder.DropIndex(
                name: "IX_GalleryImages_RoomId",
                table: "GalleryImages");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "GalleryImages");

            migrationBuilder.DropColumn(
                name: "RoomCategoryId",
                table: "GalleryImages");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "GalleryImages");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HotelId",
                table: "GalleryImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomCategoryId",
                table: "GalleryImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "GalleryImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Country", "CreatedAt", "IsDeleted", "Name", "PostalCode", "ThumbnailUrl", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("71a53161-8f7a-4ebc-87c2-87c29e5be4b4"), "Jordan", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Amman", "11118", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("71a53161-8f7a-4ebc-87c2-87c29e5be4b5"), "Turkey", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Istanbul", "34000", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Email", "FirstName", "IsDeleted", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("2a5294a7-202d-4473-84d0-3f8c2cddfac7"), "john.doe@example.com", "John", false, "Doe", "0799999999" },
                    { new Guid("2a5294a7-202d-4473-84d0-3f8c2cddfac8"), "alice.smith@example.com", "Alice", false, "Smith", "0788888888" }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "CityId", "CreatedAt", "Description", "Email", "HotelType", "IsDeleted", "Latitude", "Longitude", "Name", "OwnerId", "PhoneNumber", "StarRating", "ThumbnailUrl", "TotalRooms", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("85e91235-6799-4e62-a35b-920601e1a9db"), new Guid("71a53161-8f7a-4ebc-87c2-87c29e5be4b4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A luxurious hotel in the heart of Amman.", null, "Luxury", false, 31.953900000000001, 35.912799999999997, "Luxury Stay Amman", new Guid("2a5294a7-202d-4473-84d0-3f8c2cddfac7"), "0799999999", 5, null, 100, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("85e91235-6799-4e62-a35b-920601e1a9dc"), new Guid("71a53161-8f7a-4ebc-87c2-87c29e5be4b5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Charming seaside accommodation in Istanbul.", null, "Business", false, 41.008200000000002, 28.978400000000001, "Sea Breeze Istanbul", new Guid("2a5294a7-202d-4473-84d0-3f8c2cddfac8"), "0788888888", 4, null, 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImages_HotelId",
                table: "GalleryImages",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImages_RoomCategoryId",
                table: "GalleryImages",
                column: "RoomCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImages_RoomId",
                table: "GalleryImages",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryImages_Hotels_HotelId",
                table: "GalleryImages",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryImages_RoomCategories_RoomCategoryId",
                table: "GalleryImages",
                column: "RoomCategoryId",
                principalTable: "RoomCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryImages_Rooms_RoomId",
                table: "GalleryImages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
