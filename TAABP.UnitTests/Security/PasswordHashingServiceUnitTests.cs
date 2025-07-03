using AutoFixture;
using FluentAssertions;
using TravelAndAccommodationBookingPlatform.Infrastructure.Security.Serrvices;

namespace TAABP.UnitTests.Security;

public class PasswordHashingServiceUnitTests
{
    private readonly PasswordHashingService _passwordHashingService;
    private readonly IFixture _fixture;

    public PasswordHashingServiceUnitTests()
    {
        _fixture = new Fixture();
        _passwordHashingService = _fixture.Create<PasswordHashingService>();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = _fixture.Create<string>();

        // Act
        var hashed = _passwordHashingService.HashPassword(password);

        // Assert
        hashed.Should().NotBeNullOrWhiteSpace();
        hashed.Should().NotBe(password);
    }

    [Fact]
    public void IsPasswordVerified_ShouldReturnTrue_WhenPasswordMatches()
    {
        // Arrange
        var password = _fixture.Create<string>();
        var hashed = _passwordHashingService.HashPassword(password);

        // Act
        var result = _passwordHashingService.IsPasswordVerified(password, hashed);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsPasswordVerified_ShouldReturnFalse_WhenPasswordDoesNotMatch()
    {
        // Arrange
        var password = _fixture.Create<string>();
        var hashed = _passwordHashingService.HashPassword(password);
        var wrongPassword = _fixture.Create<string>();

        // Act
        var result = _passwordHashingService.IsPasswordVerified(wrongPassword, hashed);

        // Assert
        result.Should().BeFalse();
    }
}
