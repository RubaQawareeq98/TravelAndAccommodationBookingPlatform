using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IUserService
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByCredentialsAsync(string email, string password);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
}
