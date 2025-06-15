using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Bookings;

public class BookingRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IBookingRepository
{
    public async Task AddBooking(Booking booking)
    {
        await dbContext.Bookings.AddAsync(booking);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateBooking(Booking booking)
    {
        dbContext.Bookings.Update(booking);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteBooking(Booking booking)
    {
        dbContext.Bookings.Remove(booking);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Booking?> GetBooking(Guid id)
    {
        var booking = await dbContext.Bookings
            .Include(b => b.PaymentDetail)
            .FirstOrDefaultAsync();

        Console.WriteLine(booking?.PaymentDetail.Amount);
        return booking;
        // return await dbContext.Bookings
        //     .Include(b => b.PaymentDetail)
        //     .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Booking>> GetAllBookings(SieveModel sieveModel)
    {
        var query = dbContext.Bookings
            .Include(b => b.PaymentDetail)
            .AsQueryable();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }
}
