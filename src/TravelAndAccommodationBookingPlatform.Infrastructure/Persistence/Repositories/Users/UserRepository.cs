using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Users;

public class UserRepository(HotelBookingManagementDbContext dbContext, IUnitOfWork unitOfWork) : IUserRepository
{
    public async Task<User?> GetUserByEmail(string email)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task CreateUser(User user)
    {
        await dbContext.Users.AddAsync(user);
        await unitOfWork.SaveChanges();
    }

    public async Task UpdateUser(User user)
    {
        dbContext.Users.Update(user);
        await unitOfWork.SaveChanges();
    }
}
