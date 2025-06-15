using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Bookings;

public class BookingService(IBookingRepository bookingRepository) : IBookingService
{
    public async Task AddBooking(Booking booking)
    {
        await bookingRepository.AddBooking(booking);
    }

    public async Task UpdateBooking(Booking booking)
    {
        await bookingRepository.UpdateBooking(booking);
    }

    public async Task DeleteBooking(Guid bookingId)
    {
        var booking = await GetBookingById(bookingId);
        await bookingRepository.DeleteBooking(booking);
    }

    public async Task<Booking> GetBookingById(Guid bookingId)
    {
        var booking = await bookingRepository.GetBooking(bookingId);
        if (booking is null)
        {
            throw new NotFoundException($"Booking with id {bookingId} not found");
        }
        
        return booking;
    }

    public async Task<List<Booking>> GetBookings(SieveModel sieveModel)
    {
        return await bookingRepository.GetAllBookings(sieveModel);
    }
}
