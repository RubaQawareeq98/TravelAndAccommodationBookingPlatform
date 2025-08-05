using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using TravelAndAccommodationBookingPlatform.Application.Security.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Services.Users;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TAABP.UnitTests.Users.Services;

public class UserServiceUnitTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHashingService> _mockPasswordService;
    private readonly UserService _userService;

    public UserServiceUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _mockUserRepository = _fixture.Freeze<Mock<IUserRepository>>();
        _mockPasswordService = _fixture.Freeze<Mock<IPasswordHashingService>>();

        _userService = _fixture.Create<UserService>();
    }

    [Fact]
    public async Task GetUserById_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockUserRepository.Setup(r => r.GetUserById(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserById(user.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockUserRepository.Setup(r => r.GetUserById(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserById(userId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(UserError.UserNotFoundById(userId).Code);
    }

    [Fact]
    public async Task GetUserByCredentials_ShouldReturnUser_WhenPasswordMatches()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var inputPassword = _fixture.Create<string>();

        _mockUserRepository.Setup(r => r.GetUserByEmail(user.Email)).ReturnsAsync(user);
        _mockPasswordService.Setup(p => p.IsPasswordVerified(inputPassword, user.Password)).Returns(true);

        // Act
        var result = await _userService.GetUserByCredentials(user.Email, inputPassword);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task GetUserByCredentials_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        _mockUserRepository.Setup(r => r.GetUserByEmail(email)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserByCredentials(email, password);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(UserError.UserUnauthorized().Code);
    }

    [Fact]
    public async Task GetUserByCredentials_ShouldReturnFailure_WhenPasswordDoesNotMatch()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var password = _fixture.Create<string>();

        _mockUserRepository.Setup(r => r.GetUserByEmail(user.Email)).ReturnsAsync(user);
        _mockPasswordService.Setup(p => p.IsPasswordVerified(password, user.Password)).Returns(false);

        // Act
        var result = await _userService.GetUserByCredentials(user.Email, password);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(UserError.UserUnauthorized().Code);
    }

    [Fact]
    public async Task AddUser_ShouldReturnFailure_WhenEmailAlreadyUsed()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockUserRepository.Setup(r => r.GetUserByEmail(user.Email)).ReturnsAsync(user);

        // Act
        var result = await _userService.AddUser(user);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(UserError.EmailAlreadyUsed(user.Email).Code);
    }

    [Fact]
    public async Task AddUser_ShouldHashPassword_AndCreateUser_WhenEmailNotUsed()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var hashedPassword = _fixture.Create<string>();

        _mockUserRepository.Setup(r => r.GetUserByEmail(user.Email)).ReturnsAsync((User?)null);
        _mockPasswordService.Setup(p => p.HashPassword(user.Password)).Returns(hashedPassword);
        _mockUserRepository.Setup(r => r.CreateUser(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        var result = await _userService.AddUser(user);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be(user.Email);
        result.Value.Password.Should().Be(hashedPassword);
        _mockUserRepository.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_ShouldCallRepositoryUpdate()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockUserRepository.Setup(r => r.UpdateUser(user)).Returns(Task.CompletedTask).Verifiable();

        // Act
        await _userService.UpdateUser(user);

        // Assert
        _mockUserRepository.Verify(r => r.UpdateUser(user), Times.Once);
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnCorrectUser()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockUserRepository.Setup(r => r.GetUserByEmail(user.Email)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByEmail(user.Email);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task GetUserNameById_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var firstName = _fixture.Create<string>();
        var lastName = _fixture.Create<string>();
        var user = _fixture.Build<User>()
            .With(u => u.FirstName, firstName)
            .With(u => u.LastName, lastName)
            .Create();

        _mockUserRepository.Setup(r => r.GetUserById(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserNameById(user.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        ((Result<string>)result).Value.Should().Be($"{firstName} {lastName}");
    }

    [Fact]
    public async Task GetUserNameById_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockUserRepository.Setup(r => r.GetUserById(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserNameById(userId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(UserError.UserNotFoundById(userId).Code);
    }
}
