using Microsoft.AspNetCore.Http;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Images;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Cities;

public class CityService(ICityRepository cityRepository, IImageService imageService) : ICityService
{
    public async Task AddCityAsync(City city)
    {
        await cityRepository.AddCity(city);
    }

    public async Task UpdateCityAsync(City city)
    {
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

    public async Task<string> UpdateCityThumbnail(Guid hotelId, IFormFile file)
    {
        var city = await GetCityByIdAsync(hotelId);
        var url = await imageService.UploadImageAsync(file);
        
        city.ThumbnailUrl = url;
        await cityRepository.UpdateCity(city);
        
        return url;
    }

    public async Task<List<City>> GetTrendingCities(int listCount, CancellationToken cancellationToken = default)
    {
        if (listCount < 1)
        {
            listCount = 1;
        }
        return await cityRepository.GetMostTrendingCities(listCount, cancellationToken);
    }
}
