using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Filtering.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.RoomCategories;

public class RoomCategoryRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessorWrapper sieveProcessor,
    IUnitOfWork unitOfWork) : IRoomCategoryRepository
{
    public async Task AddRoomCategory(RoomCategory roomCategory, CancellationToken cancellationToken)
    {
        await dbContext.RoomCategories.AddAsync(roomCategory, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task UpdateRoomCategory(RoomCategory roomCategory, CancellationToken cancellationToken)
    {
        dbContext.RoomCategories.Update(roomCategory);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task DeleteRoomCategory(RoomCategory roomCategory, CancellationToken cancellationToken)
    {
        roomCategory.IsDeleted = true;
        await unitOfWork.SaveChanges(cancellationToken);
    }
    
    public async Task<RoomCategory?> GetRoomCategoryById(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.RoomCategories.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, cancellationToken);
    }

    public async Task<List<RoomCategory>> GetAllRoomCategoriesByHotelId(Guid hotelId, SieveModel sieveModel, CancellationToken cancellationToken)
    {
        var query = dbContext.RoomCategories
            .Where(r => !r.IsDeleted && r.HotelId == hotelId)
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
            .AsNoTracking();
        
        query = sieveProcessor.Apply(sieveModel, query);
        
        return await query.ToListAsync(cancellationToken);
    }
}
