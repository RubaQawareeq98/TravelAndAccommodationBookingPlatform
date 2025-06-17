using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IAmenityService
{
    Task AddAmenity(Amenity amenity);
    Task UpdateAmenity(Amenity amenity);
    Task DeleteAmenity(Guid amenityId);
    Task<Amenity> GetAmenityById(Guid amenityId);
    Task<List<Amenity>> GetAmenities(SieveModel sieveModel);
}
