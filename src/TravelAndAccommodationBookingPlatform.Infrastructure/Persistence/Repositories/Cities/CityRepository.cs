using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Filtering.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Cities;

public class CityRepository(HotelBookingManagementDbContext dbContext,
    ISieveProcessorWrapper  sieveProcessor,
    IUnitOfWork unitOfWork) : ICityRepository
{
    public async Task AddCity(City city, CancellationToken cancellationToken)
    {
        await dbContext.Cities.AddAsync(city, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task<City?> GetCityById(Guid id)
    {
        return await dbContext.Cities.FirstOrDefaultAsync(city => city.Id == id && !city.IsDeleted);
    }

    public async Task UpdateCity(City city, CancellationToken cancellationToken)
    {
        dbContext.Cities.Update(city);
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task DeleteCity(City city, CancellationToken cancellationToken)
    {
        city.IsDeleted = true;
        await unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task<bool> IsCityExist(Guid id)
    {
        return await dbContext.Cities.AnyAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<List<City>> GetCities(SieveModel sieveModel, CancellationToken cancellationToken)
    {
        var query = dbContext.Cities.AsNoTracking();
        
        query = sieveProcessor.Apply(sieveModel, query);
        
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<City>> GetMostTrendingCities(int listCount, CancellationToken cancellationToken = default)
    {
        var cityBookings = await (
                from b in dbContext.Bookings
                join h in dbContext.Hotels on b.HotelId equals h.Id
                join c in dbContext.Cities on h.CityId equals c.Id
                where !c.IsDeleted
                group b by c.Id into g
                orderby g.Count() descending
                select new 
                {
                    CityId = g.Key,
                    TotalBookings = g.Count()
                })
            .Take(listCount)
            .ToListAsync(cancellationToken);

        var trendingCities = await dbContext.Cities
            .Where(c => cityBookings.Select(cb => cb.CityId).Contains(c.Id))
            .ToListAsync(cancellationToken);

        return trendingCities;
    }

    public async Task<bool> IsCityExistByName(string name)
    {
        return await dbContext.Cities.AnyAsync(c => c.Name == name);
    }
}
