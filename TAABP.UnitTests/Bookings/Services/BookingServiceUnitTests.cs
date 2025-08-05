using AutoFixture.AutoMoq;
using TravelAndAccommodationBookingPlatform.Application.Bookings.Dtos;
using TravelAndAccommodationBookingPlatform.Application.Bookings.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Emails.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.InvoiceDocuments.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Payments.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Rooms.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;
using AutoFixture;
using FluentAssertions;
using Moq;
using TravelAndAccommodationBookingPlatform.Application.Services.Bookings;

namespace TAABP.UnitTests.Bookings.Services;

public class BookingServiceUnitTests
{
    private readonly IFixture _fixture;
    private readonly BookingService _bookingService;

    public BookingServiceUnitTests()
    {
        _fixture = new Fixture();
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _fixture.Freeze<Mock<IBookingRepository>>();
        _fixture.Freeze<Mock<IUserService>>();
        _fixture.Freeze<Mock<IEmailService>>();
        _fixture.Freeze<Mock<IInvoiceGenerator>>();
        _fixture.Freeze<Mock<IPaymentService>>();
        _fixture.Freeze<Mock<IBookingValidator>>();
        _fixture.Freeze<Mock<IRoomAvailabilityValidator>>();
        
        _bookingService = _fixture.Create<BookingService>();
    }

    [Fact]
    public async Task AddBooking_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var booking = _fixture.Create<Booking>();
        var roomIds = _fixture.CreateMany<Guid>(5).ToList();

        _fixture.Freeze<Mock<IBookingValidator>>()
            .Setup(v => v.ValidateBooking(booking.UserId, booking, roomIds))
            .ReturnsAsync(Result<BookingValidationResult>.Failure(RoomError.NoRoomsFound()));

        // Act
        var result = await _bookingService.AddBooking(booking.UserId, booking, roomIds, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(RoomError.NoRoomsFound());
    }

    [Fact]
    public async Task AddBooking_ShouldReturnFailure_WhenRoomAvailabilityFails()
    {
        // Arrange
        var booking = _fixture.Create<Booking>();
        var roomIds = _fixture.Create<List<Guid>>();
        var validationResult = _fixture.Create<BookingValidationResult>();

        _fixture.Freeze<Mock<IBookingValidator>>()
            .Setup(v => v.ValidateBooking(booking.UserId, booking, roomIds))
            .ReturnsAsync(Result<BookingValidationResult>.Success(validationResult));

        _fixture.Freeze<Mock<IRoomAvailabilityValidator>>()
            .Setup(v => v.ValidateRoomAvailability(booking, validationResult.Rooms))
            .Returns(Result.Failure(RoomError.RoomNotAvailable(roomIds[0])));

        // Act
        var result = await _bookingService.AddBooking(booking.UserId, booking, roomIds);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(RoomError.RoomNotAvailable(roomIds[0]));
    }

    [Fact]
    public async Task AddBooking_ShouldSendEmailAndReturnBooking_WhenCashPayment()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var hotel = _fixture.Create<Hotel>();
        var rooms = _fixture.Create<List<Room>>();

        var booking = _fixture.Build<Booking>()
            .With(b => b.User, user)
            .With(b => b.PaymentDetails, new PaymentDetails { PaymentMethod = PaymentMethod.Cash })
            .Without(b => b.Rooms) 
            .Create();

        var validationResult = new BookingValidationResult
        {
            User = user,
            Hotel = hotel,
            Rooms = rooms
        };

        _fixture.Freeze<Mock<IBookingValidator>>()
            .Setup(v => v.ValidateBooking(user.Id, booking, It.IsAny<List<Guid>>()))
            .ReturnsAsync(Result<BookingValidationResult>.Success(validationResult));

        _fixture.Freeze<Mock<IRoomAvailabilityValidator>>()
            .Setup(v => v.ValidateRoomAvailability(booking, rooms))
            .Returns(Result.Success());

        _fixture.Freeze<Mock<IBookingRepository>>()
            .Setup(r => r.AddBooking(booking, rooms, CancellationToken.None))
            .ReturnsAsync(booking);

        _fixture.Freeze<Mock<IInvoiceGenerator>>()
            .Setup(i => i.GenerateInvoicePdf(booking))
            .Returns([1, 2, 3]);

        _fixture.Freeze<Mock<IEmailService>>()
            .Setup(e => e.SendConfirmationEmail(user, hotel.Name, booking, It.IsAny<byte[]>()))
            .Returns(Task.CompletedTask);

        var bookingService = _fixture.Create<BookingService>();

        // Act
        var result = await bookingService.AddBooking(user.Id, booking, null, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(booking);
    }
    
    [Fact]
    public async Task GenerateInvoiceForBooking_ShouldReturnFailure_WhenBookingNotFound()
    {
        // Arrange
        var bookingId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
    
        _fixture.Freeze<Mock<IBookingRepository>>()
            .Setup(r => r.GetBookingWithDetails(userId, bookingId, CancellationToken.None))
            .ReturnsAsync(null as Booking);

        // Act
        var result = await _bookingService.GenerateInvoiceForBooking(userId, bookingId, CancellationToken.None);
    
        // Assert
        result.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public async Task GetBookingById_ShouldReturnFailure_WhenBookingNotFound()
    {
        // Arrange
        var bookingId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
        
        _fixture.Freeze<Mock<IBookingRepository>>()
            .Setup(r => r.GetBooking(userId, bookingId, CancellationToken.None)).ReturnsAsync(null as Booking);
    
        // Act
        var result = await _bookingService.GetBookingById(userId, bookingId, CancellationToken.None);
    
        // Assert
        result.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteBooking_ShouldReturnFailure_WhenNotFound()
    {
        // Arrange
        var bookingId = _fixture.Create<Guid>();
        var userId = _fixture.Create<Guid>();
    
        _fixture.Freeze<Mock<IBookingRepository>>().Setup(r => r.GetBooking(userId, bookingId, CancellationToken.None)).ReturnsAsync(null as Booking);
    
        // Act
        var result = await _bookingService.DeleteBooking(userId, bookingId, CancellationToken.None);
    
        // Assert
        result.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public async Task GetRecentlyVisitedHotels_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
    
        _fixture.Freeze<Mock<IUserService>>().Setup(u => u.GetUserById(userId))
            .ReturnsAsync(Result<User>.Failure(UserError.UserNotFoundById(userId)));
    
        // Act
        var result = await _bookingService.GetRecentlyVisitedHotels(userId, 3);
    
        // Assert
        result.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public async Task GetRecentlyVisitedHotels_ShouldReturnSuccess_WhenUserFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var user = _fixture.Create<User>();
        var bookings = _fixture.Create<List<Booking>>();
    
        _fixture.Freeze<Mock<IUserService>>().Setup(u => u.GetUserById(userId))
            .ReturnsAsync(Result<User>.Success(user));
    
        _fixture.Freeze<Mock<IBookingRepository>>().Setup(r => r.GetUserRecentlyVisitedHotels(userId, 3, CancellationToken.None))
            .ReturnsAsync(bookings);
    
        // Act
        var result = await _bookingService.GetRecentlyVisitedHotels(userId, 3);
    
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(bookings);
    }
}
