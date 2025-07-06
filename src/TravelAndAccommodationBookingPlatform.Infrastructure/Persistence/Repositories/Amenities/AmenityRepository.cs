using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Amenities;

public class AmenityRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IAmenityRepository
{
    public async Task AddAmenity(Amenity amenity, CancellationToken cancellationToken)
    {
        await dbContext.Amenities.AddAsync(amenity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAmenity(Amenity amenity, CancellationToken cancellationToken)
    {
        dbContext.Amenities.Update(amenity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAmenity(Amenity amenity, CancellationToken cancellationToken)
    {
        dbContext.Amenities.Remove(amenity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Amenity?> GetAmenityByName(string amenityName, CancellationToken cancellationToken)
    {
        return await dbContext.Amenities.FirstOrDefaultAsync(a => a.Name == amenityName, cancellationToken);
    }

    public async Task<Amenity?> GetAmenity(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Amenities.FirstOrDefaultAsync(a => a.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<List<Amenity>> GetAllAmenities(SieveModel sieveModel, CancellationToken cancellationToken)
    {
        var query = dbContext.Amenities.AsNoTracking();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }
}
