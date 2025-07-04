using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Amenities;

public class AmenityService(IAmenityRepository amenityRepository) : IAmenityService
{
    public async Task<Result<Amenity>> AddAmenity(Amenity amenity, CancellationToken cancellationToken = default)
    {
        var existAmenity = await amenityRepository.GetAmenityByName(amenity.Name, cancellationToken);
        if (existAmenity is not null)
        {
            return Result<Amenity>.Failure(AmenityError.AmenityNameAlreadyExists(amenity.Name));
        }
        
        await amenityRepository.AddAmenity(amenity, cancellationToken);
        return Result<Amenity>.Success(amenity);
    }

    public async Task UpdateAmenity(Amenity amenity, CancellationToken cancellationToken = default)
    {
        await amenityRepository.UpdateAmenity(amenity, cancellationToken);
    }

    public async Task<Result> DeleteAmenity(Guid amenityId, CancellationToken cancellationToken = default)
    {
        var result = await GetAmenityById(amenityId, cancellationToken);
        if (result.IsFailure)
        {
            return Result.Failure(AmenityError.AmenityNotFound(amenityId));

        }
        
        var amenity = result.Value;
        await amenityRepository.DeleteAmenity(amenity, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<Amenity>> GetAmenityById(Guid amenityId, CancellationToken cancellationToken = default)
    {
        var amenity = await amenityRepository.GetAmenity(amenityId, cancellationToken);
        return amenity is null ? Result<Amenity>.Failure(AmenityError.AmenityNotFound(amenityId)) : Result<Amenity>.Success(amenity);
    }

    public async Task<List<Amenity>> GetAmenities(SieveModel sieveModel, CancellationToken cancellationToken = default)
    {
        return await amenityRepository.GetAllAmenities(sieveModel, cancellationToken);
    }
}
