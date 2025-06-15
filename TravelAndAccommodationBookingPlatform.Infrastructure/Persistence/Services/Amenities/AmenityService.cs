using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Amenities;

public class AmenityService(IAmenityRepository amenityRepository) : IAmenityService
{
    public async Task AddAmenity(Amenity amenity)
    {
        await amenityRepository.AddAmenity(amenity);
    }

    public async Task UpdateAmenity(Amenity amenity)
    {
        await amenityRepository.UpdateAmenity(amenity);
    }

    public async Task DeleteAmenity(Guid amenityId)
    {
        var amenity = await GetAmenityById(amenityId);
        await amenityRepository.DeleteAmenity(amenity);
    }

    public async Task<Amenity> GetAmenityById(Guid amenityId)
    {
        var amenity = await amenityRepository.GetAmenity(amenityId);
        if (amenity is null)
        {
            throw new NotFoundException($"Amenity with if {amenityId} not found");
        }
        
        return amenity;
    }

    public async Task<List<Amenity>> GetAmenities(SieveModel sieveModel)
    {
        return await amenityRepository.GetAllAmenities(sieveModel);
    }
}
