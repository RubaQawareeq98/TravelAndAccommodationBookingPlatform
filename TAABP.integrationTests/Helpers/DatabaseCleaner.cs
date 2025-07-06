using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TAABP.integrationTests.Helpers;

public static class DatabaseCleaner
{
    public static async Task ClearDatabaseAsync( WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();
        var tableNames = new[] {"Bookings", "Rooms", "RoomCategories", "Hotels", "Owners", "Cities", "Users"  };

        foreach (var table in tableNames)
        {
            await db.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}];");
        }
    }
}
