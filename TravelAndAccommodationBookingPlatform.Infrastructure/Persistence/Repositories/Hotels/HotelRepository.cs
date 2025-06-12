using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Hotels;

public class HotelRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IHotelRepository
{
    public async Task<List<Hotel?>> GetHotels(SieveModel sieveModel)
    {
        var query = dbContext.Hotels.AsQueryable();

        query = sieveProcessor.Apply(sieveModel, query);

        return await query.ToListAsync();
    }

    public async Task<Hotel?> GetHotelById(Guid hotelId)
    {
        return await dbContext.Hotels.FindAsync(hotelId);
    }

    public async Task AddHotel(Hotel hotel)
    {
        await dbContext.Hotels.AddAsync(hotel);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateHotel(Hotel hotel)
    { 
        dbContext.Update(hotel);
        await dbContext.SaveChangesAsync();
    }
}
