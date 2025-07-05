using AutoFixture;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using TAABP.UnitTests.Shared;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Users;

namespace TAABP.UnitTests.Users.Repositories;

public class UserRepositoryUnitTests : RepositoryUnitTestBase<HotelBookingManagementDbContext, User>
{
    private readonly UserRepository _userRepository;
    private readonly List<User> _users;

    public UserRepositoryUnitTests()
    {
        Fixture.Register(() =>
            new UserRepository(MockDbContext.Object, MockUnitOfWork.Object));

        _users = Fixture.CreateMany<User>(3).ToList();
        SetupMockDbSet(_users, ctx => ctx.Users);

        _userRepository = Fixture.Freeze<UserRepository>();
    }

    [Fact]
    public async Task CreateUser_ShouldAddAndSave()
    {
        // Arrange
        var user = Fixture.Create<User>();
        var cancellationToken = CancellationToken.None;

        MockDbSet.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<EntityEntry<User>>());

        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _userRepository.CreateUser(user);

        // Assert
        MockDbSet.Verify(x => x.AddAsync(user, cancellationToken), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task GetUserByEmail_ShouldReturnMatchingUser()
    {
        // Arrange
        var targetUser = _users.First();
        var targetEmail = targetUser.Email;
        
        // Act
        var result = await _userRepository.GetUserByEmail(targetEmail);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(targetEmail, result.Email);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnMatchingUser()
    {
        // Arrange
        var targetUser = _users.First();
        var targetId = targetUser.Id;
        
        // Act
        var result = await _userRepository.GetUserById(targetId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(targetId, result.Id);
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateAndSave()
    {
        // Arrange
        var user = Fixture.Create<User>();
        var cancellationToken = CancellationToken.None;

        MockDbSet.Setup(x => x.Update(user))
            .Returns(It.IsAny<EntityEntry<User>>());
        
        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _userRepository.UpdateUser(user);

        // Assert
        MockDbSet.Verify(x => x.Update(user), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }
}
