using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IBookingRepository
{
    Task AddBooking(Booking booking);
    Task UpdateBooking(Booking booking);
    Task<Booking?> GetBooking(Guid id);
    Task<List<Booking>> GetAllBookings(SieveModel sieveModel);
    Task DeleteBooking(Booking booking);
}
