using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Cities;

public class CityService(ICityRepository cityRepository) : ICityService
{
    public async Task AddCityAsync(City city)
    {
        await cityRepository.AddCity(city);
    }

    public async Task UpdateCityAsync(Guid cityId, City city)
    {
        var isCityExist = await cityRepository.IsCityExist(cityId);
        if (isCityExist is false)
        {
            throw new NotFoundException($"City with id: {cityId} does not exist.");
        }
        
        await cityRepository.UpdateCity(city);
    }

    public async Task DeleteCityAsync(Guid cityId)
    {
        var city = await GetCityByIdAsync(cityId);
        
        await cityRepository.DeleteCity(city);
    }

    public async Task<List<City>> GetCitiesAsync(SieveModel sieveModel)
    {
        return await cityRepository.GetCities(sieveModel);
    }

    public async Task<City> GetCityByIdAsync(Guid cityId)
    {
        var city = await cityRepository.GetCityById(cityId);
        if (city is null)
        {
            throw new NotFoundException($"City with id: {cityId} does not exist.");
        }
        
        return city;
    }
}
