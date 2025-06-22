using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IBookingService
{
    Task<Result<Booking>> AddBooking(Booking booking, List<Guid>? roomsIds);
    Task UpdateBooking(Booking booking);
    Task<Result<Booking>> DeleteBooking(Guid bookingId);
    Task<Result<Booking>> GetBookingById(Guid bookingId);
    Task<List<Booking>> GetBookings(SieveModel sieveModel);
    Task<Result<List<Booking>>> GetRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default);
}
