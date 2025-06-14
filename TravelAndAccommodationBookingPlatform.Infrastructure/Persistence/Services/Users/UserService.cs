using TravelAndAccommodationBookingPlatform.Application.Interfaces.Security;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Users;

public class UserService(IUserRepository userRepository, IPasswordHashingService passwordHashingService) : IUserService
{
    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        var user = await userRepository.GetUserById(userId);
        if (user is null)
        {
            throw new NotFoundException($"User with given id {userId} does not exist");
        }
        return user;
    }

    public async Task<User?> GetUserByCredentialsAsync(string email, string password)
    {
        var user = await userRepository.GetUserByEmail(email);
        if (user is null)
        {
            return null;
        }
        
        var isMatchedPassword = passwordHashingService.IsPasswordVerified(password, user.Password);
        
        return isMatchedPassword ? user : null;
    }

    public async Task AddUserAsync(User user)
    {
        var existUser = await userRepository.GetUserByEmail(user.Email);
        if (existUser is not null)
        {
            throw new EmailAlreadyExistsException($"User with email {user.Email} already exists.");
        }
        user.Password = passwordHashingService.HashPassword(user.Password);
        await userRepository.CreateUser(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await userRepository.UpdateUser(user);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await userRepository.GetUserByEmail(email);
    }
}
