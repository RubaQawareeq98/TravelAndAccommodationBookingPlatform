using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using TAABP.UnitTests.Shared;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Hotels;

namespace TAABP.UnitTests.Hotels.Repositories;

public class HotelRepositoryUnitTests : RepositoryUnitTestBase<HotelBookingManagementDbContext, Hotel>
{
    private readonly HotelRepository _hotelRepository;

    public HotelRepositoryUnitTests()
    {
        var hotels = Fixture.CreateMany<Hotel>(3).ToList();
        SetupMockDbSet(hotels, ctx => ctx.Hotels);
        SetupSieveProcessor();

        Fixture.Register(() =>
            new HotelRepository(MockDbContext.Object, MockSieveProcessorWrapper.Object, MockUnitOfWork.Object));

        _hotelRepository = Fixture.Create<HotelRepository>();
    }

    [Fact]
    public async Task GetAllHotels_ShouldReturnAllHotels()
    {
        var sieveModel = Fixture.Create<SieveModel>();

        var result = await _hotelRepository.GetHotels(sieveModel, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task AddHotel_ShouldAddAndSaveChanges()
    {
        // Arrange
        var hotel = Fixture.Create<Hotel>();

        // Act
        await _hotelRepository.AddHotel(hotel, CancellationToken.None);

        // Assert
        MockDbSet.Verify(d => d.AddAsync(hotel, It.IsAny<CancellationToken>()), Times.Once);
        MockUnitOfWork.Verify(u => u.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHotelById_ShouldReturnHotel_IfExistsAndNotDeleted()
    {
        // Arrange
        var hotelId = Fixture.Create<Guid>();
        var hotel = Fixture.Build<Hotel>()
            .With(x => x.Id, hotelId)
            .With(x => x.IsDeleted, false)
            .Create();
        var hotels = Fixture.CreateMany<Hotel>(2).ToList();
        hotels.Insert(0, hotel);

        SetupMockDbSet(hotels, ctx => ctx.Hotels);

        // Act
        var result = await _hotelRepository.GetHotelById(hotelId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(hotelId);
    }

    [Fact]
    public async Task GetHotelById_ShouldReturnNull_IfDeleted()
    {
        // Arrange
        var hotelId = Fixture.Create<Guid>();
        var hotel = Fixture.Build<Hotel>()
            .With(x => x.Id, hotelId)
            .With(x => x.IsDeleted, true)
            .Create();
        List<Hotel> hotels = [hotel];
        SetupMockDbSet(hotels, ctx => ctx.Hotels);

        // Act
        var result = await _hotelRepository.GetHotelById(hotelId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetHotelById_ShouldReturnNull_IfNotFound()
    {
        // Arrange
        var hotels = Fixture.Build<Hotel>().With(c => c.IsDeleted, false).CreateMany(3).ToList();
        SetupMockDbSet(hotels, ctx => ctx.Hotels);
        var hotelId = Fixture.Create<Guid>();

        // Act
        var result = await _hotelRepository.GetHotelById(hotelId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateHotel_ShouldCallUpdateAndSave()
    {
        // Arrange
        var hotel = Fixture.Create<Hotel>();
        var cancellationToken = CancellationToken.None;

        MockDbSet.Setup(x => x.Update(hotel))
            .Returns(It.IsAny<EntityEntry<Hotel>>());
        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _hotelRepository.UpdateHotel(hotel, cancellationToken);

        // Assert
        MockDbSet.Verify(x => x.Update(hotel), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task IsHotelExist_ShouldReturnTrue_IfExistsAndNotDeleted()
    {
        // Arrange
        var id = Fixture.Create<Guid>();
        var hotels = Fixture.CreateMany<Hotel>(2).ToList();
        hotels[0].Id = id;
        hotels[0].IsDeleted = false;

        SetupMockDbSet(hotels, ctx => ctx.Hotels);

        // Act
        var result = await _hotelRepository.IsHotelExists(id, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsHotelExist_ShouldReturnFalse_IfNotExistsOrDeleted()
    {
        // Arrange
        var id = Fixture.Create<Guid>();
        var hotels = Fixture.CreateMany<Hotel>(2).ToList();
        hotels.ForEach(c => c.IsDeleted = true);

        SetupMockDbSet(hotels, ctx => ctx.Hotels);

        // Act
        var result = await _hotelRepository.IsHotelExists(id, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetFeaturedDealsHotels_ShouldReturnTopDiscountedRooms()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var hotelId = Fixture.Create<Guid>();
        var roomId = Fixture.Create<Guid>();
        var cityId = Fixture.Create<Guid>();

        var discount = Fixture.Build<Discount>()
            .With(d => d.RoomCategoryId, roomId)
            .With(d => d.StartDate, now.AddDays(-1))
            .With(d => d.EndDate, now.AddDays(1))
            .Create();

        var roomCategory = Fixture.Build<RoomCategory>()
            .With(r => r.Id, roomId)
            .With(r => r.HotelId, hotelId)
            .With(r => r.Discounts, new List<Discount> { discount })
            .Create();

        var hotel = Fixture.Build<Hotel>()
            .With(h => h.Id, hotelId)
            .With(h => h.CityId, cityId)
            .Create();

        var city = Fixture.Build<City>()
            .With(c => c.Id, cityId)
            .Create();

        var mockRoomCategories = new List<RoomCategory> { roomCategory }
            .AsQueryable()
            .BuildMockDbSet();
        var mockHotels = new List<Hotel> { hotel }
            .AsQueryable()
            .BuildMockDbSet();
        var mockCities = new List<City> { city }
            .AsQueryable()
            .BuildMockDbSet();

        MockDbContext.Setup(x => x.RoomCategories).Returns(mockRoomCategories.Object);
        MockDbContext.Setup(x => x.Hotels).Returns(mockHotels.Object);
        MockDbContext.Setup(x => x.Cities).Returns(mockCities.Object);

        // Act
        var result = await _hotelRepository.GetFeaturedDealsHotels(5, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result[0].Id.Should().Be(roomId);
        result[0].Discounts.Should().ContainSingle(d => d.DiscountPercentage == discount.DiscountPercentage);
        result[0].Hotel.Should().NotBeNull();
        result[0].Hotel.City.Should().NotBeNull();
    }
}
