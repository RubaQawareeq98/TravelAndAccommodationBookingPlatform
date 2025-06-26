using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Sieve.Models;
using TAABP.UnitTests.Shared;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Cities;

namespace TAABP.UnitTests.Cities.Repositories;

public class CityRepositoryUnitTests : RepositoryUnitTestBase<HotelBookingManagementDbContext, City>
{
    private readonly CityRepository _cityRepository;
    public CityRepositoryUnitTests()
    {
        var cities = Fixture.CreateMany<City>(3).ToList();
        SetupMockDbSet(cities, ctx => ctx.Cities);
        SetupSieveProcessor();

        Fixture.Register(() =>
            new CityRepository(MockDbContext.Object, MockSieveProcessorWrapper.Object, MockUnitOfWork.Object));
        
        _cityRepository = Fixture.Create<CityRepository>();
    }
    
    [Fact]
    public async Task GetAllCities_ShouldReturnAllCities()
    {
        // Arrange
        var sieveModel = Fixture.Create<SieveModel>();

        // Act
        var result = await _cityRepository.GetCities(sieveModel, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task AddCity_ShouldAddAndSave()
    {
        // Arrange
        var city = Fixture.Create<City>();
        var cancellationToken = CancellationToken.None;

        MockDbSet.Setup(x => x.AddAsync(It.IsAny<City>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<EntityEntry<City>>());

        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _cityRepository.AddCity(city, cancellationToken);

        // Assert
        MockDbSet.Verify(x => x.AddAsync(city, cancellationToken), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetCityById_ShouldReturnCity_IfExistsAndNotDeleted()
    {
        // Arrange
        var cityId = Fixture.Create<Guid>();
        var city = Fixture.Build<City>()
            .With(x => x.Id, cityId)
            .With(x => x.IsDeleted, false)
            .Create();
        var cities = Fixture.CreateMany<City>(2).ToList();
        cities.Insert(0, city);

        SetupMockDbSet(cities, ctx => ctx.Cities);

        // Act
        var result = await _cityRepository.GetCityById(cityId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cityId);
    }

    [Fact]
    public async Task GetCityById_ShouldReturnNull_IfDeleted()
    {
        // Arrange
        var cityId = Fixture.Create<Guid>();
        var city = Fixture.Build<City>()
            .With(x => x.Id, cityId)
            .With(x => x.IsDeleted, true)
            .Create();
        List<City> cities = [city];
        SetupMockDbSet(cities, ctx => ctx.Cities);

        // Act
        var result = await _cityRepository.GetCityById(cityId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCityById_ShouldReturnNull_IfNotFound()
    {
        // Arrange
        var cities = Fixture.Build<City>().With(c => c.IsDeleted, false).CreateMany(3).ToList();
        SetupMockDbSet(cities, ctx => ctx.Cities);

        // Act
        var result = await _cityRepository.GetCityById(Fixture.Create<Guid>());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCity_ShouldCallUpdateAndSave()
    {
        // Arrange
        var city = Fixture.Create<City>();
        var cancellationToken = CancellationToken.None;

        MockDbSet.Setup(x => x.Update(city))
            .Returns(It.IsAny<EntityEntry<City>>());
        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _cityRepository.UpdateCity(city, cancellationToken);

        // Assert
        MockDbSet.Verify(x => x.Update(city), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteCity_ShouldMarkDeletedAndSave()
    {
        // Arrange
        var city = Fixture.Build<City>().With(c => c.IsDeleted, false).Create();
        var cancellationToken = CancellationToken.None;

        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken)).ReturnsAsync(1);

        // Act
        await _cityRepository.DeleteCity(city, cancellationToken);

        // Assert
        city.IsDeleted.Should().BeTrue();
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task IsCityExist_ShouldReturnTrue_IfExistsAndNotDeleted()
    {
        // Arrange
        var id = Fixture.Create<Guid>();
        var cities = Fixture.CreateMany<City>(2).ToList();
        cities[0].Id = id;
        cities[0].IsDeleted = false;

        SetupMockDbSet(cities, ctx => ctx.Cities);

        // Act
        var result = await _cityRepository.IsCityExist(id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsCityExist_ShouldReturnFalse_IfNotExistsOrDeleted()
    {
        // Arrange
        var id = Fixture.Create<Guid>();
        var cities = Fixture.CreateMany<City>(2).ToList();
        cities.ForEach(c => c.IsDeleted = true);

        SetupMockDbSet(cities, ctx => ctx.Cities);

        // Act
        var result = await _cityRepository.IsCityExist(id);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsCityExistByName_ShouldReturnTrue_IfExists()
    {
        // Arrange
        var name = Fixture.Create<string>();
        var cities = Fixture.CreateMany<City>(2).ToList();
        cities[0].Name = name;

        SetupMockDbSet(cities, ctx => ctx.Cities);

        // Act
        var result = await _cityRepository.IsCityExistByName(name);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsCityExistByName_ShouldReturnFalse_IfNotExists()
    {
        // Arrange
        var name = Fixture.Create<string>();
        var cities = Fixture.CreateMany<City>(2).ToList();

        SetupMockDbSet(cities, ctx => ctx.Cities);

        // Act
        var result = await _cityRepository.IsCityExistByName(name);

        // Assert
        result.Should().BeFalse();
    }
}
