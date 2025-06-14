using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IOwnerService
{
    Task AddOwnerAsync(Owner owner);
    Task UpdateOwnerAsync(Owner owner);
    Task DeleteOwnerAsync(Guid ownerId);
    Task<Owner> GetOwnerByIdAsync(Guid ownerId);
    Task<List<Owner>> GetOwnersAsync(SieveModel sieveModel);
}
