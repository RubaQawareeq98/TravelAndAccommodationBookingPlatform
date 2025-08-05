using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Images.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Services.Hotels;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TAABP.UnitTests.Hotels.Services;

public class HotelServiceUnitTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IHotelRepository> _hotelRepositoryMock;
    private readonly Mock<ICityService> _cityServiceMock;
    private readonly Mock<IGalleryImageService> _galleryImageServiceMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly HotelService _hotelService;

    public HotelServiceUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _hotelRepositoryMock = _fixture.Freeze<Mock<IHotelRepository>>();
        _cityServiceMock = _fixture.Freeze<Mock<ICityService>>();
        _galleryImageServiceMock = _fixture.Freeze<Mock<IGalleryImageService>>();
        _imageServiceMock = _fixture.Freeze<Mock<IImageService>>();

        _hotelService = _fixture.Create<HotelService>();
    }

    [Fact]
    public async Task AddHotel_ShouldReturnSuccess_WhenCityExists()
    {
        // Arrange
        var hotel = _fixture.Create<Hotel>();
        if (hotel.City != null)
            _cityServiceMock.Setup(r => r.GetCityById(hotel.CityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<City>.Success(hotel.City));

        // Act
        var result = await _hotelService.AddHotel(hotel);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _hotelRepositoryMock.Verify(r => r.AddHotel(hotel, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddHotel_ShouldReturnFailure_WhenCityDoesNotExist()
    {
        // Arrange
        var hotel = _fixture.Create<Hotel>();
        _cityServiceMock.Setup(r => r.GetCityById(hotel.CityId, It.IsAny<CancellationToken>())).ReturnsAsync(Result<City>.Failure(CityError.CityNotFound(hotel.CityId)));

        // Act
        var result = await _hotelService.AddHotel(hotel);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateHotel_ShouldReturnSuccess_WhenHotelExists()
    {
        // Arrange
        var hotel = _fixture.Create<Hotel>();
        _hotelRepositoryMock.Setup(x => x.IsHotelExists(hotel.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _hotelService.UpdateHotel(hotel);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _hotelRepositoryMock.Verify(x => x.UpdateHotel(hotel, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateHotel_ShouldReturnFailure_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotel = _fixture.Create<Hotel>();
        _hotelRepositoryMock.Setup(x => x.IsHotelExists(hotel.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _hotelService.UpdateHotel(hotel);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task GetHotelById_ShouldReturnHotel_WhenFound()
    {
        // Arrange
        var hotel = _fixture.Create<Hotel>();
        _hotelRepositoryMock.Setup(x => x.GetHotelById(hotel.Id, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);

        // Act
        var result = await _hotelService.GetHotelById(hotel.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(hotel);
    }

    [Fact]
    public async Task GetHotelById_ShouldReturnFailure_WhenNotFound()
    {
        // Arrange
        var hotelId = _fixture.Create<Guid>();
        _hotelRepositoryMock.Setup(x => x.GetHotelById(hotelId, It.IsAny<CancellationToken>())).ReturnsAsync(null as Hotel);

        // Act
        var result = await _hotelService.GetHotelById(hotelId);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task AddHotelGallery_ShouldReturnSuccess()
    {
        // Arrange
        var hotel = _fixture.Create<Hotel>();
        var mockFile = new Mock<IFormFile>();
        var fileName = "image.jpg";
        mockFile.Setup(f => f.FileName).Returns(fileName);

        _hotelRepositoryMock.Setup(x => x.GetHotelById(hotel.Id, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);
        _galleryImageServiceMock.Setup(x => x.AddGalleryImage(hotel.Id, mockFile.Object)).ReturnsAsync("/images/" + fileName);

        // Act
        var result = await _hotelService.AddHotelGallery(hotel.Id, mockFile.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Contain(fileName);
    }

    [Fact]
    public async Task UpdateHotelThumbnail_ShouldReturnSuccess()
    {
        // Arrange
        var hotel = _fixture.Create<Hotel>();
        var mockFile = new Mock<IFormFile>();
        var imagePath = "/newimage.jpg";
        _hotelRepositoryMock.Setup(x => x.GetHotelById(hotel.Id, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);
        _imageServiceMock.Setup(x => x.UploadImageAsync(mockFile.Object)).ReturnsAsync(imagePath);

        // Act
        var result = await _hotelService.UpdateHotelThumbnail(hotel.Id, mockFile.Object);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("/newimage.jpg");
        _hotelRepositoryMock.Verify(x => x.UpdateHotel(It.Is<Hotel>(h => h.ThumbnailUrl == "/newimage.jpg"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHotelGallery_ShouldReturnImages_WhenHotelExists()
    {
        // Arrange
        var hotel = _fixture.Create<Hotel>();
        var images = _fixture.Create<List<GalleryImage>>();
        _hotelRepositoryMock.Setup(x => x.GetHotelById(hotel.Id, It.IsAny<CancellationToken>())).ReturnsAsync(hotel);
        _galleryImageServiceMock.Setup(x => x.GetAllImagesByEntityId(hotel.Id)).ReturnsAsync(images);

        // Act
        var result = await _hotelService.GetHotelGallery(hotel.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(images);
    }

    [Fact]
    public async Task GetTopFeaturedDealsHotels_ShouldReturnList()
    {
        // Arrange
        var deals = _fixture.Create<List<RoomCategory>>();
        _hotelRepositoryMock.Setup(x => x.GetFeaturedDealsHotels(5, It.IsAny<CancellationToken>())).ReturnsAsync(deals);

        // Act
        var result = await _hotelService.GetTopFeaturedDealsHotels(5);

        // Assert
        result.Should().BeEquivalentTo(deals);
    }

    [Fact]
    public async Task GetFilteredRooms_ShouldReturnList()
    {
        // Arrange
        var list = _fixture.Create<List<RoomCategory>>();
        var sieveModel = new SieveModel();
        var amenityIds = _fixture.Create<List<Guid>>();

        _hotelRepositoryMock
            .Setup(x => x.GetFilteredRoomCategoriesWithHotel(sieveModel, amenityIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        // Act
        var result = await _hotelService.GetFilteredRooms(sieveModel, amenityIds);

        // Assert
        result.Should().BeEquivalentTo(list);
    }
}