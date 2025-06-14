using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IBookingService
{
    Task AddBooking(Booking booking);
    Task UpdateBooking(Booking booking);
    Task DeleteBooking(Guid bookingId);
    Task<Booking> GetBookingById(Guid bookingId);
    Task<List<Booking>> GetBookings(SieveModel sieveModel);
}
