using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesSeed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00fbab16-76e6-4e19-02ed-08ddb330969c"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Email", "FirstName", "IsDeleted", "LastName", "Password", "PhoneNumber", "Role" },
                values: new object[] { new Guid("00fbab16-76e6-4e19-02ed-08ddb330969b"), null, "taap.admin@gmail.com", "Taap", false, "Admin", "$2a$11$8e408FqZzbA4f50erX7K4.T1ZT5KIgS/Fd/Tx5HCQDanSaeeR/vSq", "08888888888", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00fbab16-76e6-4e19-02ed-08ddb330969b"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Email", "FirstName", "IsDeleted", "LastName", "Password", "PhoneNumber", "Role" },
                values: new object[] { new Guid("00fbab16-76e6-4e19-02ed-08ddb330969c"), null, "taap.admin@gmail.com", "Taap", false, "Admin", "$2a$11$8e408FqZzbA4f50erX7K4.T1ZT5KIgS/Fd/Tx5HCQDanSaeeR/vSq", "08888888888", "Admin" });
        }
    }
}
