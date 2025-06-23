using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface ICityRepository
{
    Task AddCity(City city, CancellationToken cancellationToken);
    Task<City?> GetCityById(Guid id);
    Task UpdateCity(City city, CancellationToken cancellationToken);
    Task DeleteCity(City city, CancellationToken cancellationToken);
    Task<bool> IsCityExist(Guid id);
    Task<List<City>> GetCities(SieveModel sieveModel, CancellationToken cancellationToken);
    Task<List<City>> GetMostTrendingCities(int listCount, CancellationToken cancellationToken = default);
    Task<bool> IsCityExistByName(string name);
}
