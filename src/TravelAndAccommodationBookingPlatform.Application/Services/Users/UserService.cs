using TravelAndAccommodationBookingPlatform.Application.Security.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Application.Services.Users;

public class UserService(IUserRepository userRepository,
    IPasswordHashingService passwordHashingService) : IUserService
{
    public async Task<Result<User>> GetUserById(Guid userId)
    {
        var user = await userRepository.GetUserById(userId);
        return user is null ? Result<User>.Failure(UserError.UserNotFoundById(userId)) : Result<User>.Success(user);
    }

    public async Task<Result<User>> GetUserByCredentials(string email, string password)
    {
        var user = await userRepository.GetUserByEmail(email);
        if (user is null)
        {
            return Result<User>.Failure(UserError.UserUnauthorized());
        }
        
        var isMatchedPassword = passwordHashingService.IsPasswordVerified(password, user.Password);
        return !isMatchedPassword ? Result<User>.Failure(UserError.UserUnauthorized()) : Result<User>.Success(user);
    }

    public async Task<Result<User>> AddUser(User user)
    {
        var existUser = await userRepository.GetUserByEmail(user.Email);
        if (existUser is not null)
        {
            return Result<User>.Failure(UserError.EmailAlreadyUsed(user.Email));
        }
        user.Password = passwordHashingService.HashPassword(user.Password);
        await userRepository.CreateUser(user);
        return Result<User>.Success(user);
    }

    public async Task UpdateUser(User user)
    {
        await userRepository.UpdateUser(user);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await userRepository.GetUserByEmail(email);
    }

    public async Task<Result> GetUserNameById(Guid userId)
    {
        var user = await userRepository.GetUserById(userId);
        
        return user is null
            ? Result.Failure(UserError.UserNotFoundById(userId))
            : Result<string>.Success($"{user.FirstName} {user.LastName}");
    }
}
