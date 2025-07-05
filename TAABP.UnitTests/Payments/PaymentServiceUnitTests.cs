using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Stripe;
using TravelAndAccommodationBookingPlatform.Application.Payments.Dtos;
using TravelAndAccommodationBookingPlatform.Infrastructure.Payments.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Payments.Services;

namespace TAABP.UnitTests.Payments;

public class PaymentServiceUnitTests
{
    private readonly Mock<PaymentIntentService> _mockPaymentIntentService;
    private readonly IFixture _fixture;
    private readonly StripePaymentService _stripePaymentService;

    public PaymentServiceUnitTests()
    {
        _fixture = new Fixture();

        var mockOptions = new Mock<IOptions<StripeSettings>>();
        mockOptions.Setup(o => o.Value).Returns(new StripeSettings { ApiKey = _fixture.Create<string>() });

        _mockPaymentIntentService = new Mock<PaymentIntentService>();

        _stripePaymentService = new StripePaymentService(mockOptions.Object, _mockPaymentIntentService.Object);
    }

    [Fact]
    public async Task CreatePaymentService_ShouldReturnClientSecret_WhenPaymentSucceeds()
    {
        // Arrange
        var clientSecret = _fixture.Create<string>();

        _mockPaymentIntentService
            .Setup(p => p.CreateAsync(
                It.IsAny<PaymentIntentCreateOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PaymentIntent { ClientSecret = clientSecret });

        var request = _fixture.Create<AddPaymentRequest>();

        // Act
        var result = await _stripePaymentService.CreatePaymentService(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(clientSecret, result.Value);
    }

    [Fact]
    public async Task CreatePaymentService_ShouldThrowStripeException_WhenStripeFails()
    {
        // Arrange
        var expectedMessage = _fixture.Create<string>();

        _mockPaymentIntentService
            .Setup(p => p.CreateAsync(
                It.IsAny<PaymentIntentCreateOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new StripeException(expectedMessage));

        var request = _fixture.Create<AddPaymentRequest>();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<StripeException>(() => _stripePaymentService.CreatePaymentService(request));
        Assert.Equal(expectedMessage, exception.Message);
    }
}
