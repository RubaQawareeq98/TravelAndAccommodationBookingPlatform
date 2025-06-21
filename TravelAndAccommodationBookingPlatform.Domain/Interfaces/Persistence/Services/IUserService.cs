using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IUserService
{
    Task<Result<User>> GetUserById(Guid userId);
    Task<Result<User>> GetUserByCredentials(string email, string password);
    Task<Result<User>> AddUser(User user);
    Task UpdateUser(User user);
    Task<User?> GetUserByEmail(string email);
    Task<Result> GetUserNameById(Guid userId);
}
