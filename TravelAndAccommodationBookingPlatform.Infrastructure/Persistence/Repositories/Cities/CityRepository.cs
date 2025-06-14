using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Cities;

public class CityRepository(HotelBookingManagementDbContext dbContext, ISieveProcessor sieveProcessor) : ICityRepository
{
    public async Task AddCity(City city)
    {
        await dbContext.Cities.AddAsync(city);
        await dbContext.SaveChangesAsync();
    }

    public async Task<City?> GetCityById(Guid id)
    {
        return await dbContext.Cities.FirstOrDefaultAsync(city => city.Id == id && !city.IsDeleted);
    }

    public async Task UpdateCity(City city)
    {
        dbContext.Cities.Update(city);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteCity(City city)
    {
        city.IsDeleted = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsCityExist(Guid id)
    {
        return await dbContext.Cities.AnyAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<List<City>> GetCities(SieveModel sieveModel)
    {
        var query = dbContext.Cities.AsQueryable();
        
        query = sieveProcessor.Apply(sieveModel, query);
        
        return await query.ToListAsync();
    }
}
