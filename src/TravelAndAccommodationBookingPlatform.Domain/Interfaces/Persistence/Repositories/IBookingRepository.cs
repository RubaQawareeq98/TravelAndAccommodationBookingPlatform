using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IBookingRepository
{
    Task<Result<Booking>> AddBooking(Booking booking, List<Room> rooms, CancellationToken cancellationToken);
    Task UpdateBooking(Booking booking);
    Task<Booking?> GetBooking(Guid userId, Guid hotelId, CancellationToken cancellationToken);
    Task<Booking?> GetBookingWithDetails(Guid userId, Guid hotelId, CancellationToken cancellationToken);
    Task<List<Booking>> GetAllBookings(SieveModel sieveModel, Guid userId, CancellationToken cancellationToken);
    Task DeleteBooking(Booking booking);
    Task<List<Booking>> GetUserRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default);
}
