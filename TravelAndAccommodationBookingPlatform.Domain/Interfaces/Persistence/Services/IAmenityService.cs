using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IAmenityService
{
    Task<Result<Amenity>> AddAmenity(Amenity amenity, CancellationToken cancellationToken = default);
    Task UpdateAmenity(Amenity amenity, CancellationToken cancellationToken = default);
    Task<Result<Amenity>> DeleteAmenity(Guid amenityId, CancellationToken cancellationToken = default);
    Task<Result<Amenity>> GetAmenityById(Guid amenityId, CancellationToken cancellationToken = default);
    Task<List<Amenity>> GetAmenities(SieveModel sieveModel, CancellationToken cancellationToken = default);
}
