using Microsoft.AspNetCore.Http;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface ICityService
{
    Task<Result<City>> AddCity(City city, CancellationToken cancellationToken = default);
    Task<Result<City>> UpdateCity(City city, CancellationToken cancellationToken = default);
    Task<Result> DeleteCity(Guid cityId, CancellationToken cancellationToken = default);
    Task<List<City>> GetCities(SieveModel sieveModel, CancellationToken cancellationToken = default);
    Task<Result<City>> GetCityById(Guid cityId, CancellationToken cancellationToken = default);
    Task<Result<City>> UpdateCityThumbnail(Guid cityId, IFormFile file, CancellationToken cancellationToken = default);
    Task<List<City>> GetTrendingCities(int listCount, CancellationToken cancellationToken = default);
}
