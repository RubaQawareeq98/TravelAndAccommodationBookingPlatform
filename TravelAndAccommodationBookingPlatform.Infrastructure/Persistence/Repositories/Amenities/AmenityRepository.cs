using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Amenities;

public class AmenityRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IAmenityRepository
{
    public async Task AddAmenity(Amenity amenity)
    {
        await dbContext.Amenities.AddAsync(amenity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAmenity(Amenity amenity)
    {
        dbContext.Amenities.Update(amenity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAmenity(Amenity amenity)
    {
        dbContext.Amenities.Remove(amenity);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<Amenity?> GetAmenity(Guid id)
    {
        return await dbContext.Amenities.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Amenity>> GetAllAmenities(SieveModel sieveModel)
    {
        var query = dbContext.Amenities.AsQueryable();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }
}
