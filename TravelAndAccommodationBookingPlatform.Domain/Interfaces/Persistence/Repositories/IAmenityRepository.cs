using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IAmenityRepository
{
    Task AddAmenity(Amenity amenity);
    Task UpdateAmenity(Amenity amenity);
    Task<Amenity?> GetAmenity(Guid id);
    Task<List<Amenity>> GetAllAmenities(SieveModel sieveModel);
    Task DeleteAmenity(Amenity amenity);
}
