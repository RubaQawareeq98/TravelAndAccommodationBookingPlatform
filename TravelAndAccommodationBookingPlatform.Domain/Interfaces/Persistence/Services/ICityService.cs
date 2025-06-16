using Microsoft.AspNetCore.Http;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface ICityService
{
    Task AddCityAsync(City city);
    Task UpdateCityAsync(City city);
    Task DeleteCityAsync(Guid cityId);
    Task<List<City>> GetCitiesAsync(SieveModel sieveModel);
    Task<City> GetCityByIdAsync(Guid cityId);
    Task<string> UpdateCityThumbnail(Guid hotelId, IFormFile file);
}
