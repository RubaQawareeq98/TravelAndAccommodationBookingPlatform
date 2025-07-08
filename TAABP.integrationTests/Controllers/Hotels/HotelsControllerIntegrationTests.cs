using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TAABP.integrationTests.Controllers.Cities.Utils;
using TAABP.integrationTests.Controllers.Discounts.Utils;
using TAABP.integrationTests.Fixtures;
using TAABP.integrationTests.Handlers;
using TAABP.integrationTests.Helpers;
using TAABP.integrationTests.Controllers.Hotels.Utils;
using TAABP.integrationTests.Controllers.RoomCategories.Utils;
using TAABP.integrationTests.Shared;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TAABP.integrationTests.Controllers.Hotels;

public class HotelsControllerIntegrationTests : IClassFixture<SqlServerFixture>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly IFixture _fixture = new Fixture();
    private const string BaseUrl = "/api/hotels";
    private readonly WebApplicationFactory<Program> _factory;

    public HotelsControllerIntegrationTests(SqlServerFixture sqlServerFixture)
    {
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

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
    public async Task GetHotels_ShouldReturnListOfHotels_WhenHotelsExist()
    {
        // Arrange
        var hotels = _fixture.Build<Hotel>()
            .Without(h => h.Bookings)
            .Without(h => h.RoomCategories)
            .Without(h => h.Gallery)
            .Without(h => h.Reviews)
            .CreateMany(3)
            .ToList();
        await HotelTestUtilities.AddTestHotels(hotels, _factory);
        
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);
    
        // Act
        var response = await _client.GetAsync(BaseUrl);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var hotelResponses = await response.Content.ReadFromJsonAsync<List<HotelResponse>>();
        hotelResponses.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetHotelById_ShouldReturnHotel_WhenHotelExists()
    {
        // Arrange
        var hotel = _fixture.Build<Hotel>()
            .With(h => h.IsDeleted, false)
            .Without(h => h.Bookings)
            .Without(h => h.RoomCategories)
            .Without(h => h.Gallery)
            .Without(h => h.Reviews)
            .Create();
        await HotelTestUtilities.AddTestHotels([hotel], _factory);
    
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);
    
        // Act
        var response = await _client.GetAsync($"{BaseUrl}/{hotel.Id}");
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var hotelResponse = await response.Content.ReadFromJsonAsync<HotelResponse>();
        hotelResponse.Should().NotBeNull();
        hotelResponse.Id.Should().Be(hotel.Id);
    }
    
    [Fact]
    public async Task GetHotelById_ShouldReturnNotFound_WhenHotelDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);
    
        // Act
        var response = await _client.GetAsync($"{BaseUrl}/{nonExistingId}");
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task AddHotel_ShouldReturnCreated_WhenValidRequest()
    {
        // Arrange
        var request = _fixture.Build<AddHotelRequest>()
            .With(h => h.StarRating, 4)       
            .With(h => h.Longitude, 35.0)          
            .With(h => h.Latitude, 32.0)                       
            .With(h => h.TotalRooms, 100)                       
            .With(h => h.HotelType, HotelType.Business)                
            .Create();
        
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);
    
        // Act
        var response = await _client.PostAsJsonAsync(BaseUrl, request);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var hotelResponse = await response.Content.ReadFromJsonAsync<HotelResponse>();
        hotelResponse.Should().NotBeNull();
        hotelResponse.Name.Should().Be(request.Name);
    }
    
    [Fact]
    public async Task CreateHotel_ShouldReturnForbidden_WhenUserNotAdmin()
    {
        // Arrange
        TestAuthenticationHeader.SetTestAuthHeader(_client, Guid.NewGuid(), UserRole.User);
    
        var request = _fixture.Build<AddHotelRequest>().Create();
    
        // Act
        var response = await _client.PostAsJsonAsync(BaseUrl, request);
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task GetFeaturedDealsHotels_ShouldReturnTopDeals_WhenDiscountsExist()
    {
        // Arrange
        var city = _fixture.Build<City>()
            .With(c => c.IsDeleted, false)
            .Without(c => c.Hotels)
            .Create();
    
        await CityTestUtilities.AddTestCities([city], _factory);
    
        var hotel = _fixture.Build<Hotel>()
            .With(h => h.CityId, city.Id)
            .With(h => h.IsDeleted, false)
            .Without(h => h.RoomCategories)
            .Without(h => h.Bookings)
            .Without(h => h.Gallery)
            .Without(h => h.Reviews)
            .Create();
    
        await HotelTestUtilities.AddTestHotels([hotel], _factory);
    
        var roomCategory = _fixture.Build<RoomCategory>()
            .With(r => r.HotelId, hotel.Id)
            .With(r => r.PricePerNight, 200m)
            .Without(r => r.Hotel)
            .Without(r => r.Rooms)
            .Without(r => r.Discounts)
            .Create();
    
        await RoomCategoryTestUtilities.AddTestRoomCategories([roomCategory], _factory);
    
        var discount = _fixture.Build<Discount>()
            .With(d => d.RoomCategoryId, roomCategory.Id)
            .With(d => d.StartDate, DateTime.UtcNow.AddDays(-1))
            .With(d => d.EndDate, DateTime.UtcNow.AddDays(2))
            .With(d => d.DiscountPercentage, 25)
            .Without(d => d.RoomCategory)
            .Create();
    
        await DiscountTestUtilities.AddTestDiscounts([discount], _factory);
        
        // Act
        var response = await _client.GetAsync($"{BaseUrl}/featured-deals?ListCount=5");
    
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    
        var featuredDeals = await response.Content.ReadFromJsonAsync<List<HotelFeaturedDealResponse>>();
        featuredDeals.Should().NotBeNull();
        featuredDeals.Should().HaveCount(1);
        featuredDeals[0].Name.Should().Be(hotel.Name);
        featuredDeals[0].DiscounPercentage.Should().Be(25);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await DatabaseCleaner.ClearDatabaseAsync(_factory);
    }
}
