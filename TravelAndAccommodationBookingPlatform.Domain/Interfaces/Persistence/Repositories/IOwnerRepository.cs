using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IOwnerRepository
{
    Task AddOwner(Owner owner);
    Task UpdateOwner(Owner owner);
    Task DeleteOwner(Owner owner);
    Task<Owner?> GetOwner(Guid id);
}
