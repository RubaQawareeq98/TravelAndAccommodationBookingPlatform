using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IAmenityRepository
{
    Task AddAmenity(Amenity room);
    Task UpdateAmenity(Amenity room);
    Task<Amenity?> GetAmenity(Guid id);
    Task<List<Amenity>> GetAllAmenities(SieveModel sieveModel);
    Task DeleteAmenity(Amenity room);
}
