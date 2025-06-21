using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.RoomCategories;

public class RoomCategoryRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IRoomCategoryRepository
{
    public async Task AddRoomCategory(RoomCategory roomCategory)
    {
        await dbContext.RoomCategories.AddAsync(roomCategory);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateRoomCategory(RoomCategory roomCategory)
    {
        dbContext.RoomCategories.Update(roomCategory);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteRoomCategory(RoomCategory roomCategory)
    {
        roomCategory.IsDeleted = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<RoomCategory>> GetFilteredRoomCategories(SieveModel sieveModel, List<Guid>? amenityIds)
    {
        var query = dbContext.RoomCategories
            .AsNoTracking()
            .Include(r => r.Amenities) 
            .Where(r => !r.IsDeleted);

        if (amenityIds is { Count: > 0 })
        {
            query = query.Where(r => amenityIds.All(id => r.Amenities.Any(a => a.Id == id)));
        }

        query = sieveProcessor.Apply(sieveModel, query);

        return await query.Select(r => new RoomCategory
        {
            Id = r.Id,
            HotelId = r.HotelId,
            Name = r.Name,
            PricePerNight = r.PricePerNight,
            AdultsCapacity = r.AdultsCapacity,
            ChildrenCapacity = r.ChildrenCapacity,
            Description = r.Description,
            Amenities = r.Amenities
        }).ToListAsync();
    }

    public async Task<RoomCategory?> GetRoomCategory(Guid id)
    {
        return await dbContext.RoomCategories.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task<List<RoomCategory>> GetAllRoomCategories()
    {
        var query = await dbContext.RoomCategories
            .AsNoTracking()
            .Where(r => !r.IsDeleted)
            .Select(r => new RoomCategory
            {
                Id = r.Id,
                HotelId = r.HotelId,
                Name = r.Name,
                PricePerNight = r.PricePerNight,
                AdultsCapacity = r.AdultsCapacity,
                ChildrenCapacity = r.ChildrenCapacity,
                Description = r.Description,
                Amenities = r.Amenities,
            })
            .ToListAsync();
        
        return query;
    }
}
