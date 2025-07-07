using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TAABP.integrationTests.Controllers.Bookings.Utils;
using TAABP.integrationTests.Controllers.Hotels.Utils;
using TAABP.integrationTests.Controllers.Users.Utils;
using TAABP.integrationTests.Fixtures;
using TAABP.integrationTests.Handlers;
using TAABP.integrationTests.Helpers;
using TAABP.integrationTests.Shared;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Responses;
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
    private readonly SqlServerFixture _sqlServerFixture;
    
    
    public BookingControllerIntegrationTests(SqlServerFixture sqlServerFixture)
    {
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
         _userId = _fixture.Create<Guid>();
        _baseUrl = $"/api/users/{_userId}/bookings";
        _sqlServerFixture = sqlServerFixture;
        _factory = sqlServerFixture.Factory;

        _client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("TestAuthScheme")
                    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandlerTest>(
                        "TestAuthScheme", _ => { });
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
        
        var hotel = _fixture.Build<Hotel>()
            .Without(u => u.Bookings)
            .Without(u => u.RoomCategories)
            .Without(u => u.Gallery)
            .Without(u => u.Reviews)
            .Create();
        
        await HotelTestUtilities.AddTestHotels([hotel], _factory);
        
        var bookings = _fixture.Build<Booking>()
            .With(b => b.UserId, user.Id)
            .With(b => b.HotelId, hotel.Id)
            .Without(b => b.PaymentDetails)
            .CreateMany(5)   
            .ToList();

        TestAuthenticationHeader.SetTestAuthHeader(_client, _userId, UserRole.Admin);
        await BookingTestUtilities.AddTestBookings(bookings, _factory);

        // Act
        var response = await _client.GetAsync($"{_baseUrl}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBookings = await response.Content.ReadFromJsonAsync<List<BookingResponse>>();
        
        // Assert
        responseBookings.Should().NotBeNull().And.HaveCount(bookings.Count);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await DatabaseCleaner.ClearDatabaseAsync(_factory);
    }
}
