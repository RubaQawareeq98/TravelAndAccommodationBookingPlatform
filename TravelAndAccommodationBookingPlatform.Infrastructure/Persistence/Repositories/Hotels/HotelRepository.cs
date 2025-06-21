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

    public async Task<List<RoomCategory>> GetFeaturedDealsHotels(int listCount, CancellationToken cancellationToken)
    {
        var currentUtc = DateTime.UtcNow;

        var discountedRoomsQuery =
            from ri in dbContext.RoomCategories.AsNoTracking()
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
            group r by r.HotelId
            into g
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

        var roomCategories = await (
            from ri in dbContext.RoomCategories.AsNoTracking()
            join h in dbContext.Hotels.AsNoTracking() on ri.HotelId equals h.Id
            join c in dbContext.Cities.AsNoTracking() on h.CityId equals c.Id
            where roomIds.Contains(ri.Id)
            select new { ri, h, c }
        ).ToListAsync(cancellationToken);

        var result = roomCategories.Select(info =>
        {
            var deal = bestDeals.First(d => d.RoomId == info.ri.Id);

            return new RoomCategory
            {
                Id = info.ri.Id,
                Name = info.ri.Name,
                Description = info.ri.Description,
                RoomType = info.ri.RoomType,
                PricePerNight = info.ri.PricePerNight,
                Hotel = new Hotel
                {
                    Id = info.h.Id,
                    Name = info.h.Name,
                    Description = info.h.Description,
                    ThumbnailUrl = info.h.ThumbnailUrl,
                    StarRating = info.h.StarRating,
                    TotalRooms = info.h.TotalRooms,
                    PhoneNumber = info.h.PhoneNumber,
                    City = new City
                    {
                        Name = info.c.Name,
                        Country = info.c.Country,
                        PostalCode = info.c.PostalCode
                    }
                },
                Discounts = new List<Discount>
                {
                    new()
                    {
                        StartDate = deal.DiscountStartDate,
                        EndDate = deal.DiscountEndDate,
                        DiscountPercentage = deal.DiscountPercentage
                    }
                }
            };
        }).ToList();

        return result;
    }
    
    public async Task<List<RoomCategory>> GetFilteredRoomCategoriesWithHotel(
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
                Hotel = new Hotel
                {
                    Name = rc.Hotel.Name,
                    Description = rc.Hotel.Description,
                    ThumbnailUrl = rc.Hotel.ThumbnailUrl,
                    City = rc.Hotel.City,
                    StarRating = rc.Hotel.StarRating
                },
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
}
