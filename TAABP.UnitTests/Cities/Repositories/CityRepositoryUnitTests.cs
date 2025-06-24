using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Filtering.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Persistence.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Cities;

namespace TAABP.UnitTests.Cities.Repositories;

public class CityRepositoryUnitTests
{
    private readonly IFixture _fixture;
    private readonly CityRepository _cityRepository;
    private readonly Mock<HotelBookingManagementDbContext> _mockDbContext;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ISieveProcessorWrapper> _mockSieveProcessorWrapper;
    private Mock<DbSet<City>> _mockDbSet;

    public CityRepositoryUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _mockUnitOfWork = _fixture.Freeze<Mock<IUnitOfWork>>();
        _mockDbContext = _fixture.Freeze<Mock<HotelBookingManagementDbContext>>();
        _mockSieveProcessorWrapper = _fixture.Freeze<Mock<ISieveProcessorWrapper>>();

        var cities = _fixture.CreateMany<City>(3).ToList();
        _mockDbSet = cities.AsQueryable().CreateMockDbSet();
        SetupCitiesDbSet(cities);
        SetupSieveProcessor();
        
        _fixture.Register(() =>
            new CityRepository(
                _mockDbContext.Object,
                _mockSieveProcessorWrapper.Object,
                _mockUnitOfWork.Object));
        
        _cityRepository = _fixture.Create<CityRepository>();
    }

    private void SetupCitiesDbSet(List<City> cities)
    {
        _mockDbSet = cities.AsQueryable().CreateMockDbSet();
        _mockDbContext.Setup(x => x.Cities).Returns(_mockDbSet.Object);
    }

    private void SetupSieveProcessor()
    {
        _mockSieveProcessorWrapper.Setup(x => x.Apply(
                It.IsAny<SieveModel>(),
                It.IsAny<IQueryable<City>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<object[]>()))
            .Returns((SieveModel _, IQueryable<City> _, bool _, bool _, object[]? _) => _mockDbSet.Object);
    }

    [Fact]
    public async Task GetAllCities_ShouldReturnAllCities()
    {
        // Arrange
        var sieveModel = _fixture.Create<SieveModel>();

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
        var city = _fixture.Create<City>();
        var cancellationToken = CancellationToken.None;

        _mockDbSet.Setup(x => x.AddAsync(It.IsAny<City>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<EntityEntry<City>>());

        _mockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _cityRepository.AddCity(city, cancellationToken);

        // Assert
        _mockDbSet.Verify(x => x.AddAsync(city, cancellationToken), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetCityById_ShouldReturnCity_IfExistsAndNotDeleted()
    {
        // Arrange
        var cityId = _fixture.Create<Guid>();
        var city = _fixture.Build<City>()
            .With(x => x.Id, cityId)
            .With(x => x.IsDeleted, false)
            .Create();
        var cities = _fixture.CreateMany<City>(2).ToList();
        cities.Insert(0, city);

        SetupCitiesDbSet(cities);

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
        var cityId = _fixture.Create<Guid>();
        var city = _fixture.Build<City>()
            .With(x => x.Id, cityId)
            .With(x => x.IsDeleted, true)
            .Create();
        List<City> cities = [city];
        SetupCitiesDbSet(cities);

        // Act
        var result = await _cityRepository.GetCityById(cityId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCityById_ShouldReturnNull_IfNotFound()
    {
        // Arrange
        var cities = _fixture.Build<City>().With(c => c.IsDeleted, false).CreateMany(3).ToList();
        SetupCitiesDbSet(cities);

        // Act
        var result = await _cityRepository.GetCityById(_fixture.Create<Guid>());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCity_ShouldCallUpdateAndSave()
    {
        // Arrange
        var city = _fixture.Create<City>();
        var cancellationToken = CancellationToken.None;

        _mockDbSet.Setup(x => x.Update(city))
            .Returns(It.IsAny<EntityEntry<City>>());
        _mockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _cityRepository.UpdateCity(city, cancellationToken);

        // Assert
        _mockDbSet.Verify(x => x.Update(city), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteCity_ShouldMarkDeletedAndSave()
    {
        // Arrange
        var city = _fixture.Build<City>().With(c => c.IsDeleted, false).Create();
        var cancellationToken = CancellationToken.None;

        _mockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken)).ReturnsAsync(1);

        // Act
        await _cityRepository.DeleteCity(city, cancellationToken);

        // Assert
        city.IsDeleted.Should().BeTrue();
        _mockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task IsCityExist_ShouldReturnTrue_IfExistsAndNotDeleted()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var cities = _fixture.CreateMany<City>(2).ToList();
        cities[0].Id = id;
        cities[0].IsDeleted = false;

        SetupCitiesDbSet(cities);

        // Act
        var result = await _cityRepository.IsCityExist(id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsCityExist_ShouldReturnFalse_IfNotExistsOrDeleted()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var cities = _fixture.CreateMany<City>(2).ToList();
        cities.ForEach(c => c.IsDeleted = true);

        SetupCitiesDbSet(cities);

        // Act
        var result = await _cityRepository.IsCityExist(id);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsCityExistByName_ShouldReturnTrue_IfExists()
    {
        // Arrange
        var name = _fixture.Create<string>();
        var cities = _fixture.CreateMany<City>(2).ToList();
        cities[0].Name = name;

        SetupCitiesDbSet(cities);

        // Act
        var result = await _cityRepository.IsCityExistByName(name);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsCityExistByName_ShouldReturnFalse_IfNotExists()
    {
        // Arrange
        var name = _fixture.Create<string>();
        var cities = _fixture.CreateMany<City>(2).ToList();

        SetupCitiesDbSet(cities);

        // Act
        var result = await _cityRepository.IsCityExistByName(name);

        // Assert
        result.Should().BeFalse();
    }
}
