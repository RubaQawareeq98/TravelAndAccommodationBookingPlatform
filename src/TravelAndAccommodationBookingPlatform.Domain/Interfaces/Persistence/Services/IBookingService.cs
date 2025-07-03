using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IBookingService
{
    Task<Result<Booking>> AddBooking(Guid userId, Booking booking, List<Guid>? roomIds, CancellationToken cancellationToken = default);
    Task UpdateBooking(Booking booking);
    Task<Result<Booking>> DeleteBooking(Guid userId, Guid bookingId, CancellationToken cancellationToken = default);
    Task<Result<Booking>> GetBookingById(Guid userId, Guid bookingId, CancellationToken cancellationToken = default);
    Task<Result<Booking>> GetBookingWithDetailsById(Guid userId, Guid bookingId, CancellationToken cancellationToken = default);
    Task<Result<List<Booking>>> GetBookings(SieveModel sieveModel, Guid userId, CancellationToken cancellationToken = default);
    Task<Result<List<Booking>>> GetRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default);
    Task<Result<byte[]>> GenerateInvoiceForBooking(Guid userId, Guid bookingId, CancellationToken cancellationToken = default);
}
