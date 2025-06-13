using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Hotels;

public class HotelRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IHotelRepository
{
    public async Task<List<Hotel>> GetHotels(SieveModel sieveModel)
    {
        var query = dbContext.Hotels.AsQueryable();

        query = sieveProcessor.Apply(sieveModel, query);

        return await query.ToListAsync();
    }

    public async Task<Hotel?> GetHotelById(Guid hotelId)
    {
        return await dbContext.Hotels.FirstOrDefaultAsync(hotel => hotel.Id == hotelId && !hotel.IsDeleted);
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

    public async Task<bool> IsHotelExists(Guid hotelId)
    {
        return await dbContext.Hotels.AnyAsync(hotel => hotel.Id == hotelId && !hotel.IsDeleted);
    }
}
