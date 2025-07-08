using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Filtering.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Owners;

public class OwnerRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessorWrapper sieveProcessor,
    IUnitOfWork unitOfWork) : IOwnerRepository
{
    public async Task AddOwner(Owner owner)
    {
        await dbContext.Owners.AddAsync(owner);
        await unitOfWork.SaveChanges();
    }

    public async Task UpdateOwner(Owner owner)
    {
        dbContext.Owners.Update(owner);
        await unitOfWork.SaveChanges();
    }

    public async Task DeleteOwner(Owner owner)
    {
        owner.IsDeleted = true;
        await unitOfWork.SaveChanges();
    }

    public async Task<Owner?> GetOwner(Guid id)
    {
        return await dbContext.Owners.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task<List<Owner>> GetOwners(SieveModel sieveModel)
    {
        var query = dbContext.Owners.AsNoTracking();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }
}
