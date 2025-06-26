using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TAABP.integrationTests.Bookings.Utils;

public class BookingTestUtilities
{
    public static async Task AddTestBookings(List<Booking> bookings, WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();
        
        dbContext.Bookings.AddRange(bookings);
        await dbContext.SaveChangesAsync();
    }
}
