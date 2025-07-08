using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using TAABP.integrationTests.Controllers.Bookings.Utils;
using TAABP.integrationTests.Controllers.Cities.Utils;
using TAABP.integrationTests.Controllers.Hotels.Utils;
using TAABP.integrationTests.Controllers.Owners;
using TAABP.integrationTests.Controllers.Users.Utils;
using TAABP.integrationTests.Fixtures;
using TAABP.integrationTests.Handlers;
using TAABP.integrationTests.Helpers;
using TAABP.integrationTests.Shared;
using TravelAndAccommodationBookingPlatform.Api;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Application.Images.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TAABP.integrationTests.Controllers.Cities;

public class CityControllerIntegrationTests : IClassFixture<SqlServerFixture>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly IFixture _fixture = new Fixture();
    private const string BaseUrl = "/api/cities";
    private readonly WebApplicationFactory<Program> _factory;
    private readonly SqlServerFixture _sqlServerFixture;
    
    
    public CityControllerIntegrationTests(SqlServerFixture sqlServerFixture)
    {
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
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
    public async Task GetCities_ShouldReturnsOkWithCities_IfValidRequest()
    {
        // Arrange
        var cities = _fixture.Build<City>()
            .Without(c => c.Hotels)
            .With(c => c.IsDeleted, false)
            .CreateMany(5)
            .ToList();

        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);
        await CityTestUtilities.AddTestCities(cities, _factory);

        // Act
        var response = await _client.GetAsync($"{BaseUrl}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseCities = await response.Content.ReadFromJsonAsync<List<CityResponse>>();
        
        // Assert
        responseCities.Should().NotBeNull().And.HaveCount(cities.Count);
    }

    [Fact]
    public async Task GetCityById_ShouldReturnsOk_IfValidCityId()
    {
        // Arrange
        var city = _fixture.Build<City>().Without(c => c.Hotels).With(c => c.IsDeleted, false).Create();
        await CityTestUtilities.AddTestCities([city], _factory);

        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);
        
        // Act
        var response = await _client.GetAsync($"{BaseUrl}/{city.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CityResponse>();
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(city.Id);
    }

    [Fact]
    public async Task GetCityById_ShouldReturnsNotFound_IfInvalidCityId()
    {
        // Arrange
        var invalidCityId = _fixture.Create<Guid>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);

        // Act
        var response = await _client.GetAsync($"{BaseUrl}/{invalidCityId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddCity_ShouldReturnsCreated_IfValidRequest()
    {
        // Arrange
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);
        var request = _fixture.Create<AddCityRequest>();

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CityResponse>();
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task DeleteCity_ShouldReturnsNoContent_IfValidCityId()
    {
        // Arrange
        var city = _fixture.Build<City>().Without(c => c.Hotels).With(c => c.IsDeleted, false).Create();
        await CityTestUtilities.AddTestCities([city], _factory);
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);

        // Act
        var response = await _client.DeleteAsync($"{BaseUrl}/{city.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteCity_ShouldReturnsNotFound_IfInvalidCityId()
    {
        // Arrange
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);
        var invalidCityId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"{BaseUrl}/{invalidCityId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateCity_ShouldReturnsNoContent_IfValidPatch()
    {
        // Arrange
        var city = _fixture.Build<City>()
            .Without(c => c.Hotels)
            .With(c => c.IsDeleted, false)
            .Create();
        var updatedName = _fixture.Create<string>();

        await CityTestUtilities.AddTestCities([city], _factory);
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);

        var patchDoc = new JsonPatchDocument<UpdateCityRequest>();
        patchDoc.Replace(c => c.Name, updatedName);
        patchDoc.Replace(c => c.Country, city.Country);
        patchDoc.Replace(c => c.PostalCode, city.PostalCode);

        var content = new StringContent(JsonConvert.SerializeObject(patchDoc), Encoding.UTF8, "application/json-patch+json");
        
        // Act
        var response = await _client.PatchAsync($"{BaseUrl}/{city.Id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateCity_ShouldReturnsNotFound_IfInvalidCityId()
    {
        // Arrange 
        var invalidCityId = _fixture.Create<Guid>();
        var updatedName = _fixture.Create<string>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);
        var patchDoc = new JsonPatchDocument<UpdateCityRequest>();
        patchDoc.Replace(c => c.Name, updatedName);

        // Act
        var content = new StringContent(JsonConvert.SerializeObject(patchDoc), Encoding.UTF8, "application/json-patch+json");
        var response = await _client.PatchAsync($"{BaseUrl}/{invalidCityId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddThumbnailToCity_ShouldReturnsOk_IfValidCityId()
    {
        // Arrange
        var mockImageService = new Mock<IImageService>();
        var imagePath = _fixture.Create<string>();
        
        mockImageService
            .Setup(x => x.UploadImageAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(imagePath);

        var factory = _sqlServerFixture.CreateFactoryWithOverrides(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IImageService));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton(mockImageService.Object);
        });

        var client = factory.CreateClient();

        var city = _fixture.Build<City>()
            .Without(c => c.Hotels)
            .With(c => c.IsDeleted, false)
            .Create();

        await CityTestUtilities.AddTestCities([city], factory);
        
        var fileContent = new StreamContent(new MemoryStream(new byte[10]));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        var content = new MultipartFormDataContent
        {
            { fileContent, "File", "thumbnail.jpg" }
        };
        
        TestAuthenticationHeader.SetTestAuthHeader(client, _fixture.Create<Guid>(), UserRole.Admin);

        // Act
        var response = await client.PutAsync($"{BaseUrl}/{city.Id}/thumbnail", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        json.TryGetProperty("imageUrl", out var imageUrl).Should().BeTrue();
        imageUrl.GetString().Should().Be(imagePath);
    }
    
    [Fact]
    public async Task AddThumbnailToCity_ShouldReturnsNotFound_IfInvalidCityId()
    {
        // Arrange
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);

        var content = new MultipartFormDataContent
        {
            { new StreamContent(new MemoryStream(new byte[10])), "File", "thumbnail.jpg" }
        };

        // Act
        var response = await _client.PutAsync($"{BaseUrl}/{Guid.NewGuid()}/thumbnail", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTrendingCities_ShouldReturnsOk()
    {
        // Arrange
        var cities = _fixture.Build<City>()
            .Without(c => c.Hotels)
            .With(c => c.IsDeleted, false)
            .CreateMany(5)
            .ToList();

        await CityTestUtilities.AddTestCities(cities, _factory);

        var owners = _fixture.Build<Owner>()
            .Without(o => o.Hotels)
            .With(o => o.IsDeleted, false)
            .CreateMany(5)
            .ToList();
        await OwnerTestUtilities.AddTestOwners(owners, _factory);

        var hotels = cities.Select((city, index) =>
        {
            return _fixture.Build<Hotel>()
                .With(h => h.CityId, city.Id)
                .With(h => h.OwnerId, owners[index].Id) 
                .Without(h => h.City)
                .Without(h => h.RoomCategories)
                .Without(h => h.Reviews)
                .Without(h => h.Gallery)
                .Without(h => h.Bookings)
                .Create();
        }).ToList();

        await HotelTestUtilities.AddTestHotels(hotels, _factory);

        var users = _fixture.Build<User>()
            .Without(u => u.Bookings)
            .CreateMany(5).ToList();
        await UserTestUtilities.AddTestUsers(users, _factory);
            
        var bookings = hotels.Select((hotel, index) =>
        {
            return _fixture.Build<Booking>()
                .With(b => b.HotelId, hotel.Id)
                .With(b => b.UserId, users[index].Id)
                .Without(b => b.Hotel)
                .Without(b => b.User)
                .Without(b => b.PaymentDetails)
                .Create();

        }).ToList();

        await BookingTestUtilities.AddTestBookings(bookings, _factory);

        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.User);

        // Act
        var response = await _client.GetAsync($"{BaseUrl}/trending?ListCount=3");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<CityResponse>>();
        result.Should().NotBeNull().And.HaveCount(3);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await DatabaseCleaner.ClearDatabaseAsync(_factory);
    }
}
