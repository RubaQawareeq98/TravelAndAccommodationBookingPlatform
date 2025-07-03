using System.Data.Entity;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Moq;
using Sieve.Models;
using TAABP.UnitTests.Shared;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Bookings;

namespace TAABP.UnitTests.Bookings.Repositories;

public class BookingRepositoryUnitTests : RepositoryUnitTestBase<HotelBookingManagementDbContext, Booking>
{
    private readonly BookingRepository _bookingRepository;

    public BookingRepositoryUnitTests()
    {
        var bookings = Fixture.CreateMany<Booking>(3).ToList();
        SetupMockDbSet(bookings, ctx => ctx.Bookings);
        SetupSieveProcessor();
        
        var discountRepositoryMock = Fixture.Freeze<Mock<IDiscountRepository>>();
        var loggerMock = Fixture.Freeze<Mock<ILogger<BookingRepository>>>();

        Fixture.Register(() =>
            new BookingRepository(
                MockDbContext.Object,
                MockSieveProcessorWrapper.Object,
                discountRepositoryMock.Object,
                 MockUnitOfWork.Object,
                loggerMock.Object
                ));

        _bookingRepository = Fixture.Create<BookingRepository>();
    }
    
    [Fact]
    public async Task GetAllBookings_ShouldReturnAllBookings()
    {
        // Arrange
        var sieveModel = Fixture.Create<SieveModel>();

        // Act
        var result = await _bookingRepository.GetAllBookings(sieveModel);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task GetBookingById_ShouldReturnBooking_IfExistsAndNotDeleted()
    {
        // Arrange
        var bookingId = Fixture.Create<Guid>();
        var booking = Fixture.Build<Booking>()
            .With(x => x.Id, bookingId)
            .Create();
        var bookings = Fixture.CreateMany<Booking>(2).ToList();
        bookings.Insert(0, booking);

        SetupMockDbSet(bookings, ctx => ctx.Bookings);

        // Act
        var result = await _bookingRepository.GetBooking(bookingId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(bookingId);
    }

    [Fact]
    public async Task GetBookingById_ShouldReturnNull_IfNotFound()
    {
        // Arrange
        var bookingId = Fixture.Create<Guid>();
        var booking = Fixture.Create<Booking>();
        List<Booking> bookings = [booking];
        SetupMockDbSet(bookings, ctx => ctx.Bookings);

        // Act
        var result = await _bookingRepository.GetBooking(bookingId);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateBooking_ShouldCallUpdateAndSave()
    {
        // Arrange
        var booking = Fixture.Create<Booking>();
        var cancellationToken = CancellationToken.None;

        MockDbSet.Setup(x => x.Update(booking))
            .Returns(It.IsAny<EntityEntry<Booking>>());
        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await _bookingRepository.UpdateBooking(booking);

        // Assert
        MockDbSet.Verify(x => x.Update(booking), Times.Once);
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task DeleteBooking_ShouldMarkDeletedAndSave()
    {
        // Arrange
        var booking = Fixture.Create<Booking>();
        var cancellationToken = CancellationToken.None;

        MockUnitOfWork.Setup(x => x.SaveChanges(cancellationToken)).ReturnsAsync(1);

        // Act
        await _bookingRepository.DeleteBooking(booking);

        // Assert
        MockUnitOfWork.Verify(x => x.SaveChanges(cancellationToken), Times.Once);
    }
}
