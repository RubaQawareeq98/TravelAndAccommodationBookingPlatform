using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Application.Services.Owners;

public class OwnerService(IOwnerRepository ownerRepository) : IOwnerService
{
    public async Task AddOwnerAsync(Owner owner)
    {
        await ownerRepository.AddOwner(owner);
    }

    public async Task UpdateOwnerAsync(Owner owner)
    {
        await ownerRepository.UpdateOwner(owner);
    }

    public async Task DeleteOwnerAsync(Guid ownerId)
    {
        var owner = await GetOwnerByIdAsync(ownerId);
        await ownerRepository.DeleteOwner(owner);
    }

    public async Task<Owner> GetOwnerByIdAsync(Guid ownerId)
    {
        var owner = await ownerRepository.GetOwner(ownerId);
        if (owner is null)
        {
            throw new NotFoundException($"Owner with if {ownerId} not found");
        }
        
        return owner;
    }

    public async Task<List<Owner>> GetOwnersAsync(SieveModel sieveModel)
    {
        return await ownerRepository.GetOwners(sieveModel);
    }
}
