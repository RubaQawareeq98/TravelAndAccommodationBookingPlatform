using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TAABP.integrationTests.Controllers.Cities.Utils;

public class CityTestUtilities
{
    public static async Task AddTestCities(List<City> cities, WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();
        
        dbContext.Cities.AddRange(cities);
        await dbContext.SaveChangesAsync();
    }
}
