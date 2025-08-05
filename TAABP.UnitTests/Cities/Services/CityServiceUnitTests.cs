using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Images.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Services.Cities;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

namespace TAABP.UnitTests.Cities.Services;

public class CityServiceUnitTests
{
    private readonly IFixture _fixture;
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly CityService _cityService;

    public CityServiceUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _cityRepositoryMock = _fixture.Freeze<Mock<ICityRepository>>();
        _imageServiceMock = _fixture.Freeze<Mock<IImageService>>();

        _cityService = _fixture.Create<CityService>();
    }
    
    [Fact]
    public async Task AddCity_ShouldReturnSuccess_WhenCityDoesNotExist()
    {
        // Arrange
        var city = _fixture.Create<City>();
        _cityRepositoryMock.Setup(r => r.IsCityExistByName(city.Name)).ReturnsAsync(false);

        // Act
        var result = await _cityService.AddCity(city);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(city);
        _cityRepositoryMock.Verify(r => r.AddCity(city, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task AddCity_ShouldReturnFailure_WhenCityNameAlreadyExists()
    {
        // Arrange
        var city = _fixture.Create<City>();
        _cityRepositoryMock.Setup(r => r.IsCityExistByName(city.Name)).ReturnsAsync(true);

        // Act
        var result = await _cityService.AddCity(city);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CityError.CityNameAlreadyExists(city.Name));
        _cityRepositoryMock.Verify(r => r.AddCity(It.IsAny<City>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetCityById_ShouldReturnSuccess_WhenCityExists()
    {
        // Arrange
        var city = _fixture.Create<City>();
        _cityRepositoryMock.Setup(r => r.GetCityById(city.Id)).ReturnsAsync(city);

        // Act
        var result = await _cityService.GetCityById(city.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(city);
    }

    [Fact]
    public async Task GetCityById_ShouldReturnFailure_WhenCityNotFound()
    {
        // Arrange
        var cityId = _fixture.Create<Guid>();
        _cityRepositoryMock.Setup(r => r.GetCityById(cityId)).ReturnsAsync((City?)null);

        // Act
        var result = await _cityService.GetCityById(cityId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CityError.CityNotFound(cityId));
    }
    
    [Fact]
    public async Task DeleteCity_ShouldDeleteCity_WhenCityExists()
    {
        // Arrange
        var city = _fixture.Create<City>();
        _cityRepositoryMock.Setup(r => r.GetCityById(city.Id)).ReturnsAsync(city);

        // Act
        var result = await _cityService.DeleteCity(city.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _cityRepositoryMock.Verify(r => r.DeleteCity(city, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCity_ShouldReturnFailure_WhenCityDoesNotExist()
    {
        // Arrange
        var cityId = _fixture.Create<Guid>();
        _cityRepositoryMock.Setup(r => r.GetCityById(cityId)).ReturnsAsync((City?)null);

        // Act
        var result = await _cityService.DeleteCity(cityId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CityError.CityNotFound(cityId));
    }
 
    [Fact]
    public async Task UpdateCity_ShouldUpdateCity_WhenCityExists()
    {
        // Arrange
        var city = _fixture.Create<City>();
        _cityRepositoryMock.Setup(r => r.GetCityById(city.Id)).ReturnsAsync(city);

        // Act
        var result = await _cityService.UpdateCity(city);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _cityRepositoryMock.Verify(r => r.UpdateCity(city, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCity_ShouldReturnFailure_WhenCityDoesNotExist()
    {
        // Arrange
        var city = _fixture.Create<City>();
        _cityRepositoryMock.Setup(r => r.GetCityById(city.Id)).ReturnsAsync((City?)null);

        // Act
        var result = await _cityService.UpdateCity(city);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CityError.CityNotFound(city.Id));
    }

    [Fact]
    public async Task UpdateCityThumbnail_ShouldUpdateThumbnail_WhenCityExists()
    {
        // Arrange
        var city = _fixture.Create<City>();
        var file = Mock.Of<IFormFile>();
        var imageUrl = _fixture.Create<string>();

        _cityRepositoryMock.Setup(r => r.GetCityById(city.Id)).ReturnsAsync(city);
        _imageServiceMock.Setup(i => i.UploadImageAsync(file)).ReturnsAsync(imageUrl);

        // Act
        var result = await _cityService.UpdateCityThumbnail(city.Id, file);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.ThumbnailUrl.Should().Be(imageUrl);
        _cityRepositoryMock.Verify(r => r.UpdateCity(city, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCityThumbnail_ShouldReturnFailure_WhenCityDoesNotExist()
    {
        // Arrange
        var cityId = _fixture.Create<Guid>();
        var file = Mock.Of<IFormFile>();

        _cityRepositoryMock.Setup(r => r.GetCityById(cityId)).ReturnsAsync((City?)null);

        // Act
        var result = await _cityService.UpdateCityThumbnail(cityId, file);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CityError.CityNotFound(cityId));
    }
    
    [Fact]
    public async Task GetCities_ShouldReturnListOfCities_WhenCalled()
    {
        // Arrange
        var sieveModel = _fixture.Create<SieveModel>();
        var cities = _fixture.CreateMany<City>(3).ToList();

        _cityRepositoryMock
            .Setup(r => r.GetCities(sieveModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cities);

        // Act
        var result = await _cityService.GetCities(sieveModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(cities.Count);
        result.Should().BeEquivalentTo(cities);
    }
    
    [Fact]
    public async Task GetTrendingCities_ShouldReturnTopTrendingCities()
    {
        // Arrange
        const int listCount = 2;
        var trendingCities = _fixture.CreateMany<City>(listCount).ToList();

        _cityRepositoryMock
            .Setup(r => r.GetMostTrendingCities(listCount, It.IsAny<CancellationToken>()))
            .ReturnsAsync(trendingCities);

        // Act
        var result = await _cityService.GetTrendingCities(listCount);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(listCount);
        result.Should().BeEquivalentTo(trendingCities);
    }
}
