using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TAABP.integrationTests.Controllers.Users.Utils;

public class UserTestUtilities
{
    public static async Task AddTestUsers(List<User> users, WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();
        
        dbContext.Users.AddRange(users);
        await dbContext.SaveChangesAsync();
    }
}
