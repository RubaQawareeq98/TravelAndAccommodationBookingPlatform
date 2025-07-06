using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Filtering.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Hotels;

public class HotelRepository(
    HotelBookingManagementDbContext dbContext,
    ISieveProcessorWrapper sieveProcessor,
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
        dbContext.Hotels.Update(hotel);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task<bool> IsHotelExists(Guid hotelId, CancellationToken cancellationToken)
    {
        return await dbContext.Hotels.AnyAsync(hotel => hotel.Id == hotelId && !hotel.IsDeleted, cancellationToken);
    }

    public async Task<List<RoomCategory>> GetFeaturedDealsHotels(int listCount, CancellationToken cancellationToken)
    {
        var currentUtc = DateTime.UtcNow;

        var discountedRooms = await (
            from ri in dbContext.RoomCategories
            from d in ri.Discounts
            where d.StartDate <= currentUtc && d.EndDate > currentUtc
            select new
            {
                RoomCategory = ri,
                Discount = d
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var bestDeals = discountedRooms
            .GroupBy(x => x.RoomCategory.HotelId)
            .Select(g => g
                .OrderByDescending(x => x.Discount.DiscountPercentage)
                .ThenBy(x => x.RoomCategory.PricePerNight)
                .First())
            .Take(listCount)
            .ToList();

        var roomIds = bestDeals.Select(x => x.RoomCategory.Id).ToList();

        var roomsWithHotel = await (
            from rc in dbContext.RoomCategories
            join h in dbContext.Hotels on rc.HotelId equals h.Id
            join c in dbContext.Cities on h.CityId equals c.Id
            where roomIds.Contains(rc.Id)
            select new
            {
                rc.Id,
                rc.Name,
                RoomDescription = rc.Description,
                HotelDescription = h.Description,
                rc.RoomType,
                rc.PricePerNight,
                HotelId = h.Id,
                HotelName = h.Name,
                h.ThumbnailUrl,
                h.StarRating,
                h.TotalRooms,
                h.PhoneNumber,
                CityName = c.Name,
                c.Country,
                c.PostalCode
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var result = roomsWithHotel.Select(info =>
        {
            var deal = bestDeals.First(d => d.RoomCategory.Id == info.Id);

            return new RoomCategory
            {
                Id = info.Id,
                Name = info.Name,
                Description = info.RoomDescription,
                RoomType = info.RoomType,
                PricePerNight = info.PricePerNight,
                Hotel = new Hotel
                {
                    Id = info.HotelId,
                    Name = info.HotelName,
                    Description = info.HotelDescription,
                    ThumbnailUrl = info.ThumbnailUrl,
                    StarRating = info.StarRating,
                    TotalRooms = info.TotalRooms,
                    PhoneNumber = info.PhoneNumber,
                    City = new City
                    {
                        Name = info.CityName,
                        Country = info.Country,
                        PostalCode = info.PostalCode
                    }
                },
                Discounts = new List<Discount>
                {
                    new()
                    {
                        StartDate = deal.Discount.StartDate,
                        EndDate = deal.Discount.EndDate,
                        DiscountPercentage = deal.Discount.DiscountPercentage
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

        query = sieveProcessor.Apply(sieveModel, query);

        var result = await query
            .Select(rc => new
            {
                rc.Id,
                rc.HotelId,
                rc.Name,
                rc.PricePerNight,
                rc.AdultsCapacity,
                rc.ChildrenCapacity,
                rc.Description,
                Amenities = rc.Amenities.Select(a => new { a.Id, a.Name }).ToList(),
                Hotel = new
                {
                    rc.Hotel.Name,
                    rc.Hotel.Description,
                    rc.Hotel.ThumbnailUrl,
                    rc.Hotel.StarRating,
                    City = new
                    {
                        rc.Hotel.City.Name,
                        rc.Hotel.City.Country,
                        rc.Hotel.City.PostalCode
                    }
                }
            })
            .ToListAsync(cancellationToken); 

        if (amenityIds is { Count: > 0 })
        {
            result = result
                .Where(rc => amenityIds.All(id => rc.Amenities.Any(a => a.Id == id)))
                .ToList();
        }

        return result.Select(rc => new RoomCategory
        {
            Id = rc.Id,
            HotelId = rc.HotelId,
            Name = rc.Name,
            PricePerNight = rc.PricePerNight,
            AdultsCapacity = rc.AdultsCapacity,
            ChildrenCapacity = rc.ChildrenCapacity,
            Description = rc.Description,
            Hotel = new Hotel
            {
                Name = rc.Hotel.Name,
                Description = rc.Hotel.Description,
                ThumbnailUrl = rc.Hotel.ThumbnailUrl,
                StarRating = rc.Hotel.StarRating,
                City = new City
                {
                    Name = rc.Hotel.City.Name,
                    Country = rc.Hotel.City.Country,
                    PostalCode = rc.Hotel.City.PostalCode
                }
            },
            Amenities = rc.Amenities
                .Where(a => amenityIds == null || amenityIds.Contains(a.Id))
                .Select(a => new Amenity { Id = a.Id, Name = a.Name })
                .ToList()
        }).ToList();
    }
}
