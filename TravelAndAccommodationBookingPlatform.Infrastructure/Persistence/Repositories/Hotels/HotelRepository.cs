using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Persistence;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Hotels;

public class HotelRepository(
    HotelBookingManagementDbContext dbContext,
    ISieveProcessor sieveProcessor,
    IUnitOfWork unitOfWork) : IHotelRepository
{
    public async Task<List<Hotel>> GetHotels(SieveModel sieveModel, CancellationToken cancellationToken)
    {
        var query = dbContext
            .Hotels
            .Where(h => !h.IsDeleted)
            .AsNoTracking();

        query = sieveProcessor.Apply(sieveModel, query);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Hotel?> GetHotelById(Guid hotelId, CancellationToken cancellationToken)
    {
        return await dbContext.Hotels.FirstOrDefaultAsync(hotel => hotel.Id == hotelId && !hotel.IsDeleted, cancellationToken: cancellationToken);
    }

    public async Task AddHotel(Hotel hotel, CancellationToken cancellationToken)
    {
        await dbContext.Hotels.AddAsync(hotel, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task UpdateHotel(Hotel hotel, CancellationToken cancellationToken)
    {
        dbContext.Update(hotel);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task<bool> IsHotelExists(Guid hotelId, CancellationToken cancellationToken)
    {
        return await dbContext.Hotels.AnyAsync(hotel => hotel.Id == hotelId && !hotel.IsDeleted, cancellationToken);
    }

    public async Task<List<RoomInfo>> GetFeaturedDealsHotels(int listCount,
        CancellationToken cancellationToken)
    {
        var currentUtc = DateTime.UtcNow;

        var discountedRoomsQuery = 
            from ri in dbContext.RoomInfos.AsNoTracking()
            from d in ri.Discounts
            where d.StartDate <= currentUtc && d.EndDate > currentUtc
            select new
            {
                RoomId = ri.Id,
                ri.PricePerNight,
                ri.HotelId,
                ri.Name,
                ri.Description,
                ri.RoomType,
                d.DiscountPercentage,
                DiscountStartDate = d.StartDate,
                DiscountEndDate = d.EndDate
            };
        
        var bestDealsQuery = 
            from r in discountedRoomsQuery
            group r by r.HotelId into g
            select g
                .OrderByDescending(x => x.DiscountPercentage)
                .ThenBy(x => x.PricePerNight)
                .First();
        
        var bestDeals = await bestDealsQuery
            .Take(listCount)
            .ToListAsync(cancellationToken);

        if (bestDeals.Count == 0)
        {
            return [];
        }
        
        var roomIds = bestDeals.Select(x => x.RoomId).ToList();
    
        var roomData = await (
            from ri in dbContext.RoomInfos.AsNoTracking()
            where roomIds.Contains(ri.Id)
            join h in dbContext.Hotels.AsNoTracking() on ri.HotelId equals h.Id
            join c in dbContext.Cities.AsNoTracking() on h.CityId equals c.Id
            join bd in bestDeals on ri.Id equals bd.RoomId
            select new RoomInfo
            {
                Id = ri.Id,
                Name = ri.Name,
                Description = ri.Description,
                RoomType = ri.RoomType,
                PricePerNight = ri.PricePerNight,
                Hotel = new Hotel
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    ThumbnailUrl = h.ThumbnailUrl,
                    StarRating = h.StarRating,
                    TotalRooms = h.TotalRooms,
                    City = new City
                    {
                        Name = c.Name,
                        Country = c.Country,
                        PostalCode = c.PostalCode
                    },
                    PhoneNumber = h.PhoneNumber
                },
                Discounts = new List<Discount>
                {
                    new()
                    {
                        StartDate = bd.DiscountStartDate,
                        EndDate = bd.DiscountEndDate,
                        DiscountPercentage = bd.DiscountPercentage
                    }
                }
            }).ToListAsync(cancellationToken);
        
        return roomData;
    }
}
