using AutoFixture;
using AutoFixture.AutoMoq;
using CloudinaryDotNet.Actions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using TravelAndAccommodationBookingPlatform.Infrastructure.Images;
using TravelAndAccommodationBookingPlatform.Infrastructure.Images.CloudinaryService.Interfaces;

namespace TAABP.UnitTests.Images;

public class ImageServiceUnitTests
{
    [Fact]
    public async Task UploadImageAsync_ShouldReturnSecureUrl_WithAutoFixture()
    {
        // Arrange
        var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });

        var expectedUrl = fixture.Create<Uri>();
        var fileMock = fixture.Freeze<Mock<IFormFile>>();
        var stream = new MemoryStream("fake image"u8.ToArray());
        var fileName = fixture.Create<string>();

        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.FileName).Returns(fileName);

        var cloudinaryMock = fixture.Freeze<Mock<ICloudinaryWrapper>>();
        cloudinaryMock
            .Setup(c => c.UploadAsync(It.IsAny<ImageUploadParams>()))
            .ReturnsAsync(new ImageUploadResult { SecureUrl = expectedUrl });

        var service = new ImageService(cloudinaryMock.Object);

        // Act
        var result = await service.UploadImageAsync(fileMock.Object);

        // Assert
        result.Should().Be(expectedUrl.ToString());
    }
}
