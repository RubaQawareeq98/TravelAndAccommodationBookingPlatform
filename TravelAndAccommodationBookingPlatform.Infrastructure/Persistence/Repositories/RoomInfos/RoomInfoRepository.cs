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

    public async Task<RoomInfo?> GetRoomInfo(Guid id)
    {
        return await dbContext.RoomInfos.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task<List<RoomInfo>> GetAllRoomInfos(SieveModel sieveModel)
    {
        var query = dbContext.RoomInfos.AsQueryable();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }
}
