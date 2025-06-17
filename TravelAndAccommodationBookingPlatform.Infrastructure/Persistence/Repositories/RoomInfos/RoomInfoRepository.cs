using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.RoomInfos;

public class RoomInfoRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IRoomInfoRepository
{
    public async Task AddRoomInfo(RoomInfo roomInfo)
    {
        await dbContext.RoomInfos.AddAsync(roomInfo);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateRoomInfo(RoomInfo roomInfo)
    {
        dbContext.RoomInfos.Update(roomInfo);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteRoomInfo(RoomInfo roomInfo)
    {
        roomInfo.IsDeleted = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<RoomInfo>> GetFilteredRoomInfos(SieveModel sieveModel, List<Guid>? amenityIds)
    {
        var query = dbContext.RoomInfos
            .AsNoTracking()
            .Include(r => r.Amenities) 
            .Where(r => !r.IsDeleted);

        if (amenityIds is { Count: > 0 })
        {
            query = query.Where(r => amenityIds.All(id => r.Amenities.Any(a => a.Id == id)));
        }

        query = sieveProcessor.Apply(sieveModel, query);

        return await query.Select(r => new RoomInfo
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

    public async Task<RoomInfo?> GetRoomInfo(Guid id)
    {
        return await dbContext.RoomInfos.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task<List<RoomInfo>> GetAllRoomInfos()
    {
        var query = await dbContext.RoomInfos
            .AsNoTracking()
            .Where(r => !r.IsDeleted)
            .Select(r => new RoomInfo
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
