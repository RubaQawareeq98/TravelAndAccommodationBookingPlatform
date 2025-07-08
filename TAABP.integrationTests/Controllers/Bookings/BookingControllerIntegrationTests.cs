using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using TAABP.integrationTests.Controllers.Bookings.Utils;
using TAABP.integrationTests.Controllers.Hotels.Utils;
using TAABP.integrationTests.Controllers.RoomCategories.Utils;
using TAABP.integrationTests.Controllers.Rooms.Utils;
using TAABP.integrationTests.Controllers.Users.Utils;
using TAABP.integrationTests.Fixtures;
using TAABP.integrationTests.Handlers;
using TAABP.integrationTests.Helpers;
using TAABP.integrationTests.Shared;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Application.Emails.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TAABP.integrationTests.Controllers.Bookings;

public class BookingControllerIntegrationTests : IClassFixture<SqlServerFixture>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly IFixture _fixture = new Fixture();
    private readonly string _baseUrl;
    private readonly Guid _userId;
    private readonly WebApplicationFactory<Program> _factory;


    public BookingControllerIntegrationTests(SqlServerFixture sqlServerFixture)
    {
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _userId = _fixture.Create<Guid>();
        _baseUrl = $"/api/users/{_userId}/bookings";
        _factory = sqlServerFixture.Factory;
        
        var mockEmailService = new Mock<IEmailService>();
        mockEmailService
            .Setup(e => e.SendConfirmationEmail(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<Booking>(), It.IsAny<byte[]>()))
            .Returns(Task.CompletedTask);

        _client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("TestAuthScheme")
                    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandlerTest>(
                        "TestAuthScheme", _ => { });
                
                services.RemoveAll(typeof(IEmailService));
                services.AddScoped(_ => mockEmailService.Object);
            });
        }).CreateClient();
    }
    
    [Fact]
    public async Task GetBookings_ShouldReturnsOkWithBookings_IfValidRequest()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.Id, _userId)
            .Without(u => u.Bookings)
            .Create();
        await UserTestUtilities.AddTestUsers([user], _factory);
        
        var bookings = _fixture.Build<Booking>()
            .With(b => b.UserId, user.Id)
            .Without(b => b.User)
            .Without(b => b.PaymentDetails)
            .CreateMany(5)   
            .ToList();

        TestAuthenticationHeader.SetTestAuthHeader(_client, _userId, UserRole.User);
        await BookingTestUtilities.AddTestBookings(bookings, _factory);

        // Act
        var response = await _client.GetAsync($"{_baseUrl}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBookings = await response.Content.ReadFromJsonAsync<List<BookingResponse>>();
        
        // Assert
        responseBookings.Should().NotBeNull().And.HaveCount(bookings.Count);
    }

    [Fact]
    public async Task AddBooking_ShouldReturnCreatedAndPersistBooking_WhenValidRequest()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.Id, _userId)
            .Without(u => u.Bookings)
            .Create();
        await UserTestUtilities.AddTestUsers([user], _factory);

        var hotel = _fixture.Build<Hotel>()
            .Without(h => h.RoomCategories)
            .Without(h => h.Reviews)
            .Without(h => h.Gallery)
            .Without(h => h.Bookings)
            .Create();
        await HotelTestUtilities.AddTestHotels([hotel], _factory);
        
        var roomCategory = _fixture.Build<RoomCategory>()
            .With(r => r.HotelId, hotel.Id)
            .Without(r => r.Hotel)
            .Without(r => r.Discounts)
            .Without(r => r.Amenities)
            .Without(r => r.Rooms)
            .Create();
        
        await RoomCategoryTestUtilities.AddTestRoomCategories([roomCategory], _factory);

        var rooms = _fixture.Build<Room>()
            .With(r => r.RoomCategoryId, roomCategory.Id)
            .Without(r => r.RoomCategory)
            .Without(r => r.Gallery)
            .Without(r => r.Bookings)
            .CreateMany(2)
            .ToList();

        await RoomTestUtilities.AddTestRooms(rooms, _factory);

        var checkInDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2));
        var checkOutDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));

        var request = _fixture.Build<AddBookingRequest>()
            .With(r => r.CheckInDate, checkInDate)
            .With(r => r.CheckOutDate, checkOutDate)
            .With(r => r.PaymentMethod, PaymentMethod.Cash.ToString())
            .With(r => r.HotelId, hotel.Id)
            .With(r => r.RoomsIds, rooms.Select(r => r.Id).ToList())
            .Create();
        
        TestAuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.User);

        var url = $"/api/users/{user.Id}/bookings";

        // Act
        var response = await _client.PostAsJsonAsync(url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var bookingResponse = await response.Content.ReadFromJsonAsync<BookingResponse>();
        bookingResponse.Should().NotBeNull();
        bookingResponse.UserId.Should().Be(user.Id);
        bookingResponse.HotelId.Should().Be(hotel.Id);
    }


    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await DatabaseCleaner.ClearDatabaseAsync(_factory);
    }
}
