using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Rooms;

public class RoomRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : IRoomRepository
{
    public async Task AddRoom(Room room)
    {
        await dbContext.Rooms.AddAsync(room);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateRoom(Room room)
    {
        dbContext.Rooms.Update(room);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteRoom(Room room)
    {
        room.IsDeleted = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Room>> GetRoomsByRoomsIds(List<Guid> roomIds)
    {
        var rooms = await dbContext.Rooms
            .AsNoTracking()
            .Where(r => roomIds.Contains(r.Id) && !r.IsDeleted)
            .Select(r => new Room
            {
                Id = r.Id,
                RoomInfo = new RoomInfo
                {
                    Id = r.RoomInfo.Id,
                    HotelId = r.RoomInfo.HotelId
                },
                Bookings = r.Bookings
                    .Select(b => new Booking
                    {
                        CheckInDate = b.CheckInDate,
                        CheckOutDate = b.CheckOutDate
                    })
                    .ToList()
            })
            .AsSplitQuery()
            .ToListAsync();

        return rooms;
    }

    public async Task<Room?> GetRoom(Guid id)
    {
        return await dbContext.Rooms
            .Include(r => r.RoomInfo)
            .Include(r => r.Bookings)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }

    public async Task<List<Room>> GetAllRooms(SieveModel sieveModel)
    {
        var query = dbContext.Rooms.AsQueryable();
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }
}
