using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Hotels;

public class HotelRepository(
    HotelBookingManagementDbContext dbContext,
    ISieveProcessor sieveProcessor) : IHotelRepository
{
    public async Task<List<Hotel>> GetHotels(SieveModel sieveModel)
    {
        var query = dbContext
            .Hotels
            .Where(h => !h.IsDeleted)
            .AsNoTracking();

        query = sieveProcessor.Apply(sieveModel, query);

        return await query.ToListAsync();
    }

    public async Task<Hotel?> GetHotelById(Guid hotelId)
    {
        return await dbContext.Hotels.FirstOrDefaultAsync(hotel => hotel.Id == hotelId && !hotel.IsDeleted);
    }

    public async Task AddHotel(Hotel hotel)
    {
        await dbContext.Hotels.AddAsync(hotel);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateHotel(Hotel hotel)
    {
        dbContext.Update(hotel);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsHotelExists(Guid hotelId)
    {
        return await dbContext.Hotels.AnyAsync(hotel => hotel.Id == hotelId && !hotel.IsDeleted);
    }

    public async Task<List<RoomInfo>> GetFeaturedDealsHotels(int listCount,
        CancellationToken cancellationToken = default)
    {
        var currentUtc = DateTime.UtcNow;

        var rooms = await (
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
            }
        ).ToListAsync(cancellationToken);

        var bestDeals = rooms
            .GroupBy(x => x.HotelId)
            .Select(g => g
                .OrderByDescending(x => x.DiscountPercentage)
                .ThenBy(x => x.PricePerNight)
                .First())
            .Take(listCount)
            .ToList();

        var roomIds = bestDeals.Select(x => x.RoomId).ToList();

        var roomData = await (
            from ri in dbContext.RoomInfos.AsNoTracking()
            where roomIds.Contains(ri.Id)
            join h in dbContext.Hotels.AsNoTracking() on ri.HotelId equals h.Id
            join c in dbContext.Cities.AsNoTracking() on h.CityId equals c.Id
            select new
            {
                RoomId = ri.Id,
                RoomName = ri.Name,
                ri.RoomType,
                HotelName = h.Name,
                HotelId = h.Id,
                h.PhoneNumber,
                HotelDescription = h.Description,
                h.ThumbnailUrl,
                h.StarRating,
                h.TotalRooms,
                CityName = c.Name,
                CountryName = c.Country,
                c.PostalCode,
            }
        ).ToListAsync(cancellationToken);

        var result = roomData
            .Join(bestDeals,
                rd => rd.RoomId,
                bd => bd.RoomId,
                (rd, bd) =>
                    new RoomInfo
                    {
                        Hotel = new Hotel
                        {
                            Id = rd.HotelId,
                            Name = rd.HotelName,
                            Description = rd.HotelDescription,
                            ThumbnailUrl = rd.ThumbnailUrl,
                            StarRating = rd.StarRating,
                            TotalRooms = rd.TotalRooms,
                            City = new City
                            {
                                Name = rd.CityName,
                                Country = rd.CountryName,
                                PostalCode = rd.PostalCode
                            },
                            PhoneNumber = rd.PhoneNumber
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
                    }).ToList();

        return result;
    }
}
