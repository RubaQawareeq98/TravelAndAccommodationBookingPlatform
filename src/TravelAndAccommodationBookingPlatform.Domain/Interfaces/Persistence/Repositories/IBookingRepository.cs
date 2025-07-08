using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IBookingRepository
{
    Task<Booking> AddBooking(Booking booking, List<Room> rooms, CancellationToken cancellationToken);
    Task UpdateBooking(Booking booking);
    Task<Booking?> GetBooking(Guid userId, Guid bookingId, CancellationToken cancellationToken);
    Task<Booking?> GetBookingWithDetails(Guid userId, Guid bookingId, CancellationToken cancellationToken);
    Task<List<Booking>> GetUserBookings(SieveModel sieveModel, Guid userId, CancellationToken cancellationToken);
    Task DeleteBooking(Booking booking);
    Task<List<Booking>> GetUserRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default);
}
