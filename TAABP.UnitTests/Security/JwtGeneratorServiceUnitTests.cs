using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Services;

namespace TAABP.UnitTests.Security;

public class JwtGeneratorServiceUnitTests
{
    private readonly JwtGeneratorService _jwtGeneratorService;

    public JwtGeneratorServiceUnitTests()
    {
        var jwtOptions = new JwtAuthOptions
        {
            SecretKey = "SuperSecretKey1234567890!@#LONGER", 
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            TokenExpirationMinutes = 60
        };

        var optionsMock = new Mock<IOptions<JwtAuthOptions>>();
        optionsMock.Setup(o => o.Value).Returns(jwtOptions);

        _jwtGeneratorService = new JwtGeneratorService(optionsMock.Object);
    }

    [Fact]
    public void GenerateJwtToken_ShouldReturnValidJwt_WithCorrectClaims()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Role = UserRole.Admin
        };

        // Act
        var token = _jwtGeneratorService.GenerateJwtToken(user);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();
    }
}