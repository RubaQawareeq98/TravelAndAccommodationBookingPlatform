using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Owners;

public class OwnerRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IOwnerRepository
{
    public async Task AddOwner(Owner owner)
    {
        await dbContext.Owners.AddAsync(owner);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateOwner(Owner owner)
    {
        dbContext.Owners.Update(owner);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteOwner(Owner owner)
    {
        owner.IsDeleted = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task<Owner?> GetOwner(Guid id)
    {
        return await dbContext.Owners.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task<List<Owner>> GetOwners(SieveModel sieveModel)
    {
        var query = dbContext.Owners.AsQueryable();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }
}
