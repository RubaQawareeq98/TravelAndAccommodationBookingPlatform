using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TAABP.integrationTests.Controllers.Owners.Utis;

public class OwnerTestUtilities
{
    public static async Task AddTestOwners(List<Owner> owners, WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();
        
        dbContext.Owners.AddRange(owners);
        await dbContext.SaveChangesAsync();
    }
}
