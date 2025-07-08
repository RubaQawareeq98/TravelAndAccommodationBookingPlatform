using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TAABP.integrationTests.Controllers.RoomCategories.Utils;

public class RoomCategoryTestUtilities
{
    public static async Task AddTestRoomCategories(List<RoomCategory> roomCategories, WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();
        
        dbContext.RoomCategories.AddRange(roomCategories);
        await dbContext.SaveChangesAsync();
    }
}
