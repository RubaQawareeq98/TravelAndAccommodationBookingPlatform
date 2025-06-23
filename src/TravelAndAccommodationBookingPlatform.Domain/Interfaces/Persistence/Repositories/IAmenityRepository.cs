using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IAmenityRepository
{
    Task AddAmenity(Amenity amenity, CancellationToken cancellationToken);
    Task UpdateAmenity(Amenity amenity, CancellationToken cancellationToken);
    Task<Amenity?> GetAmenity(Guid id, CancellationToken cancellationToken);
    Task<List<Amenity>> GetAllAmenities(SieveModel sieveModel, CancellationToken cancellationToken);
    Task DeleteAmenity(Amenity amenity, CancellationToken cancellationToken);
    Task<Amenity?> GetAmenityByName(string amenityName, CancellationToken cancellationToken);
}
