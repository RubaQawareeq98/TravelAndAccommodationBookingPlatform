using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IAmenityService
{
    Task AddAmenity(Amenity discount);
    Task UpdateAmenity(Amenity discount);
    Task DeleteAmenity(Guid discountId);
    Task<Amenity> GetAmenityById(Guid discountId);
    Task<List<Amenity>> GetAmenities(SieveModel sieveModel);
}
