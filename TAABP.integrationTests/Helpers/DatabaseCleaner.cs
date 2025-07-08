using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TAABP.integrationTests.Helpers;

public static class DatabaseCleaner
{
    public static async Task ClearDatabaseAsync(WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();

        var tableNames = await db.Database
            .SqlQueryRaw<string>(
                """
                SELECT TABLE_NAME
                              FROM INFORMATION_SCHEMA.TABLES
                              WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = 'dbo'
                """)
            .ToListAsync();

        await db.Database.ExecuteSqlRawAsync("EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");

        foreach (var table in tableNames)
        {
            await db.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}]");
        }

        await db.Database.ExecuteSqlRawAsync("EXEC sp_msforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");
    }
}
