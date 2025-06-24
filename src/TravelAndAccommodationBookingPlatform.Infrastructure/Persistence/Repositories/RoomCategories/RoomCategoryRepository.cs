using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.RoomCategories;

public class RoomCategoryRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessor sieveProcessor,
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
    
    public async Task<List<RoomCategory>> GetFilteredRoomCategories(
        SieveModel sieveModel,
        List<Guid>? amenityIds,
        CancellationToken cancellationToken)
        {
            var query = dbContext.RoomCategories
                .AsNoTracking()
                .Where(rc => !rc.IsDeleted);

            if (amenityIds is { Count: > 0 })
            {
                query = query.Where(rc =>
                    rc.Amenities
                        .Where(a => amenityIds.Contains(a.Id))
                        .Select(a => a.Id)
                        .Distinct()
                        .Count() == amenityIds.Count);
            }

            query = sieveProcessor.Apply(sieveModel, query);

            return await query
                .Select(rc => new RoomCategory
                {
                    Id = rc.Id,
                    HotelId = rc.HotelId,
                    Name = rc.Name,
                    PricePerNight = rc.PricePerNight,
                    AdultsCapacity = rc.AdultsCapacity,
                    ChildrenCapacity = rc.ChildrenCapacity,
                    Description = rc.Description,
                    Amenities = amenityIds != null && amenityIds.Count > 0
                        ? rc.Amenities
                            .Where(a => amenityIds.Contains(a.Id))
                            .ToList()
                        : new List<Amenity>()
                })
                .ToListAsync(cancellationToken);
        }
    
    public async Task<RoomCategory?> GetRoomCategoryById(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.RoomCategories.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted, cancellationToken);
    }

    public async Task<List<RoomCategory>> GetAllRoomCategoriesByHotelId(Guid hotelId, CancellationToken cancellationToken)
    {
        var query = await dbContext.RoomCategories
            .AsNoTracking()
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
            .ToListAsync(cancellationToken: cancellationToken);
        
        return query;
    }
}
