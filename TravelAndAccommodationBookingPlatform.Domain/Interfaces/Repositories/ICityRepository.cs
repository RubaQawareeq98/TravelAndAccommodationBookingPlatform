using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface ICityRepository
{
    Task AddCity(City city);
    Task<City?> GetCityById(Guid id);
    Task UpdateCity(City city);
    Task DeleteCity(City city);
    Task<bool> IsCityExist(Guid id);
    Task<List<City>> GetCities(SieveModel sieveModel);
}
