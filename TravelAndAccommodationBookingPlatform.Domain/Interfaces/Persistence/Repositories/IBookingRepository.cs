using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IBookingRepository
{
    Task<Booking> AddBooking(Booking booking, List<Room> rooms);
    Task UpdateBooking(Booking booking);
    Task<Booking?> GetBooking(Guid id);
    Task<List<Booking>> GetAllBookings(SieveModel sieveModel);
    Task DeleteBooking(Booking booking);
    Task<List<Booking>> GetUserRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default);
}
