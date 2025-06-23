using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Rooms;

public class RoomRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessor sieveProcessor,
    IUnitOfWork unitOfWork) : IRoomRepository
{
    public async Task AddRoom(Room room)
    {
        await dbContext.Rooms.AddAsync(room);
        await unitOfWork.SaveChanges();
    }

    public async Task UpdateRoom(Room room)
    {
        dbContext.Rooms.Update(room);
        await unitOfWork.SaveChanges();
    }

    public async Task DeleteRoom(Room room, CancellationToken cancellationToken)
    {
        room.IsDeleted = true;
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task<List<Room>> GetRoomsByRoomsIds(List<Guid> roomIds)
    {
        var rooms = await dbContext.Rooms
            .AsNoTracking() 
            .Where(r => roomIds.Contains(r.Id) && !r.IsDeleted)
            .Include(room => room.RoomCategory)
            .Select(r => new Room
            {
                Id = r.Id,
                RowVersion = r.RowVersion,
                RoomNumber = r.RoomNumber,
                UpdatedAt = r.UpdatedAt,
                RoomCategory = new RoomCategory
                {
                    Id = r.RoomCategory.Id,
                    Name = r.RoomCategory.Name,
                    HotelId = r.RoomCategory.HotelId,
                    PricePerNight = r.RoomCategory.PricePerNight,
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

    public async Task<Room?> GetRoomByNumber(string roomNumber, Guid roomCategoryId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RoomNumber == roomNumber && r.RoomCategoryId == roomCategoryId
                , cancellationToken);
    }

    public async Task<List<Room>> GetRoomsByRoomCategory(Guid roomCategoryId, SieveModel sieveModel, CancellationToken cancellationToken)
    {
        var query = dbContext.Rooms
            .Where(r => r.RoomCategoryId == roomCategoryId)
            .AsNoTracking();
        
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Room?> GetRoom(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Rooms
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);
    }
}
