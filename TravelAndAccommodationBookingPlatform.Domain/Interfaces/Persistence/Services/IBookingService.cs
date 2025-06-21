using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IBookingService
{
    Task<Result> AddBooking(Booking booking, List<Guid> roomsIds);
    Task UpdateBooking(Booking booking);
    Task DeleteBooking(Guid bookingId);
    Task<Booking> GetBookingById(Guid bookingId);
    Task<List<Booking>> GetBookings(SieveModel sieveModel);
    Task<List<Booking>> GetRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default);
}
