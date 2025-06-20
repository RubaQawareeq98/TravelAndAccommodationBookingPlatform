using Microsoft.AspNetCore.Http;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Images;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Cities;

public class CityService(ICityRepository cityRepository, IImageService imageService) : ICityService
{
    public async Task<Result<City>> AddCity(City city, CancellationToken cancellationToken = default)
    {
        var isCityExist = await cityRepository.IsCityExistByName(city.Name);
        if (isCityExist)
        {
            return Result<City>.Failure(CityError.AlreadyExists(city.Name));
        }
        
        await cityRepository.AddCity(city, cancellationToken);
        return Result<City>.Success(city);
    }

    public async Task UpdateCity(City city, CancellationToken cancellationToken = default)
    {
        await cityRepository.UpdateCity(city);
    }

    public async Task<Result<City>> DeleteCity(Guid cityId, CancellationToken cancellationToken = default)
    {
        var cityResult = await GetCityById(cityId, cancellationToken);
        if (cityResult.IsFailure)
        {
            return Result<City>.Failure(cityResult.Error); 
        }

        var city = cityResult.Value;
        await cityRepository.DeleteCity(city, cancellationToken);
        return Result<City>.Success(city);
    }
    
    public async Task<List<City>> GetCities(SieveModel sieveModel, CancellationToken cancellationToken = default)
    {
        return await cityRepository.GetCities(sieveModel, cancellationToken);
    }

    public async Task<Result<City>> GetCityById(Guid cityId, CancellationToken cancellationToken = default)
    {
        var city = await cityRepository.GetCityById(cityId);
        return city is null ? Result<City>.Failure(CityError.NotFound(cityId)) : Result<City>.Success(city);
    }

    public async Task<Result<City>> UpdateCityThumbnail(Guid cityId, IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var cityResult = await GetCityById(cityId, cancellationToken);
        if (cityResult.IsFailure)
        {
            return Result<City>.Failure(CityError.NotFound(cityId));
        }

        var city = cityResult.Value;
        
        var url = await imageService.UploadImageAsync(file);
        
        city.ThumbnailUrl = url;
        await cityRepository.UpdateCity(city);
        
        return Result<City>.Success(city);
    }

    public async Task<List<City>> GetTrendingCities(int listCount, CancellationToken cancellationToken = default)
    {
        return await cityRepository.GetMostTrendingCities(listCount, cancellationToken);
    }
}
