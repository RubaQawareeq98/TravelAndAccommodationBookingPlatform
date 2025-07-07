using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Rooms;

namespace TAABP.UnitTests.Rooms.Services;

public class RoomServiceUnitTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IRoomRepository> _mockRoomRepo;
    private readonly Mock<IRoomCategoryService> _mockRoomCategoryService;
    private readonly Mock<IGalleryImageService> _mockGalleryImageService;
    private readonly RoomService _roomService;

    public RoomServiceUnitTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _mockRoomRepo = _fixture.Freeze<Mock<IRoomRepository>>();
        _mockRoomCategoryService = _fixture.Freeze<Mock<IRoomCategoryService>>();
        _mockGalleryImageService = _fixture.Freeze<Mock<IGalleryImageService>>();

        _roomService = _fixture.Create<RoomService>();
    }

    [Fact]
    public async Task AddRoom_ShouldReturnFailure_WhenRoomCategoryNotFound()
    {
        // Arrange
        var room = _fixture.Create<Room>();
        var hotelId = _fixture.Create<Guid>();
        var roomCategoryId = _fixture.Create<Guid>();
        var cancellationToken = CancellationToken.None;

        _mockRoomCategoryService.Setup(s =>
            s.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId)));

        // Act
        var result = await _roomService.AddRoom(room, hotelId, roomCategoryId, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(RoomCategoryError.RoomCategoryNotFound(roomCategoryId).Code);
    }

    [Fact]
    public async Task AddRoom_ShouldReturnFailure_WhenRoomNumberExists()
    {
        // Arrange
        var room = _fixture.Create<Room>();
        var hotelId = _fixture.Create<Guid>();
        var roomCategoryId = _fixture.Create<Guid>();
        var cancellationToken = CancellationToken.None;

        var roomCategory = _fixture.Create<RoomCategory>();

        _mockRoomCategoryService.Setup(s =>
            s.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Success(roomCategory));

        _mockRoomRepo.Setup(r =>
            r.GetRoomByNumber(room.RoomNumber, roomCategoryId, cancellationToken))
            .ReturnsAsync(_fixture.Create<Room>());

        // Act
        var result = await _roomService.AddRoom(room, hotelId, roomCategoryId, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(RoomError.RoomNumberAlreadyExist(room.RoomNumber).Code);
    }

    [Fact]
    public async Task AddRoom_ShouldAddSuccessfully_WhenValid()
    {
        // Arrange
        var room = _fixture.Create<Room>();
        var hotelId = _fixture.Create<Guid>();
        var roomCategoryId = _fixture.Create<Guid>();
        var cancellationToken = CancellationToken.None;
        var roomCategory = _fixture.Create<RoomCategory>();

        _mockRoomCategoryService.Setup(s =>
            s.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Success(roomCategory));

        _mockRoomRepo.Setup(r =>
            r.GetRoomByNumber(room.RoomNumber, roomCategoryId, cancellationToken))
            .ReturnsAsync((Room?)null);

        // Act
        var result = await _roomService.AddRoom(room, hotelId, roomCategoryId, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(room);
    }

    [Fact]
    public async Task AddHotelGallery_ShouldReturnFailure_WhenRoomNotFound()
    {
        // Arrange
        var hotelId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var roomId = _fixture.Create<Guid>();
        var file = _fixture.Create<IFormFile>();
        var cancellationToken = CancellationToken.None;

        _mockRoomCategoryService.Setup(x =>
            x.GetRoomCategoryById(hotelId, categoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Failure(RoomCategoryError.RoomCategoryNotFound(categoryId)));

        // Act
        var result = await _roomService.AddRoomGallery(hotelId, categoryId, roomId, file, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task AddHotelGallery_ShouldReturnSuccess_WhenRoomExists()
    {
        // Arrange
        var file = Mock.Of<IFormFile>();
        var hotelId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var roomId = _fixture.Create<Guid>();
        var cancellationToken = CancellationToken.None;
        var imagePath = _fixture.Create<string>();

        var category = _fixture.Build<RoomCategory>()
            .With(c => c.Id, categoryId)
            .Create();

        var room = _fixture.Build<Room>()
            .With(r => r.Id, roomId)
            .With(r => r.RoomCategoryId, categoryId) 
            .Create();

        _mockRoomCategoryService.Setup(x =>
                x.GetRoomCategoryById(hotelId, categoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Success(category));

        _mockRoomRepo.Setup(x =>
                x.GetRoom(roomId, cancellationToken))
            .ReturnsAsync(room);

        _mockGalleryImageService.Setup(x =>
                x.AddGalleryImage(roomId, file))
            .ReturnsAsync(imagePath);

        // Act
        var result = await _roomService.AddRoomGallery(hotelId, categoryId, roomId, file, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(imagePath);
    }


    [Fact]
    public async Task GetHotelGallery_ShouldReturnFailure_WhenRoomNotFound()
    {
        // Arrange
        var hotelId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var roomId = _fixture.Create<Guid>();
        var cancellationToken = CancellationToken.None;

        _mockRoomCategoryService.Setup(x =>
            x.GetRoomCategoryById(hotelId, categoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Failure(RoomCategoryError.RoomCategoryNotFound(categoryId)));

        // Act
        var result = await _roomService.GetRoomGallery(hotelId, categoryId, roomId, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(HotelError.HotelNotFound(hotelId).Code);
    }

    [Fact]
    public async Task GetHotelGallery_ShouldReturnGallery_WhenRoomExists()
    {
        // Arrange
        var hotelId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var roomId = _fixture.Create<Guid>();
        var cancellationToken = CancellationToken.None;

        var roomCategory = _fixture.Build<RoomCategory>()
            .With(c => c.Id, categoryId)
            .Create();

        var room = _fixture.Build<Room>()
            .With(r => r.Id, roomId)
            .With(r => r.RoomCategoryId, categoryId)
            .Create();

        var images = _fixture.Create<List<GalleryImage>>();

        _mockRoomCategoryService.Setup(x =>
                x.GetRoomCategoryById(hotelId, categoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Success(roomCategory));

        _mockRoomRepo.Setup(x =>
                x.GetRoom(roomId, cancellationToken))
            .ReturnsAsync(room);

        _mockGalleryImageService.Setup(x =>
                x.GetAllImagesByEntityId(roomId)) 
            .ReturnsAsync(images);

        // Act
        var result = await _roomService.GetRoomGallery(hotelId, categoryId, roomId, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(images);
    }


    [Fact]
    public async Task DeleteRoom_ShouldReturnFailure_WhenRoomNotFound()
    {
        // Arrange
        var hotelId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var roomId = _fixture.Create<Guid>();
        var cancellationToken = CancellationToken.None;

        _mockRoomCategoryService.Setup(x =>
            x.GetRoomCategoryById(hotelId, categoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Failure(RoomCategoryError.RoomCategoryNotFound(categoryId)));

        // Act
        var result = await _roomService.DeleteRoom(hotelId, categoryId, roomId, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(RoomError.RoomNotFound(roomId).Code);
    }

    [Fact]
    public async Task DeleteRoom_ShouldReturnSuccess_WhenRoomExists()
    {
        // Arrange
        var hotelId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var cancellationToken = CancellationToken.None;
        var roomCategory = _fixture.Build<RoomCategory>()
            .With(c => c.Id, categoryId)
            .With(c => c.HotelId, hotelId)
            .Create();

        var room = _fixture.Build<Room>()
            .With(r => r.RoomCategoryId, categoryId)
            .Create();

        _mockRoomCategoryService.Setup(x =>
            x.GetRoomCategoryById(hotelId, categoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Success(roomCategory));

        _mockRoomRepo.Setup(x =>
            x.GetRoom(room.Id, cancellationToken))
            .ReturnsAsync(room);

        // Act
        var result = await _roomService.DeleteRoom(hotelId, categoryId, room.Id, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetRoomsByIds_ShouldReturnFailure_WhenNoRoomsFound()
    {
        // Arrange
        var ids = _fixture.Create<List<Guid>>();

        _mockRoomRepo.Setup(x => x.GetRoomsByRoomsIds(ids))
            .ReturnsAsync([]);

        // Act
        var result = await _roomService.GetRoomsByIds(ids);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task GetRoomsByIds_ShouldReturnSuccess_WhenRoomsExist()
    {
        // Arrange
        var ids = _fixture.Create<List<Guid>>();
        var rooms = _fixture.Create<List<Room>>();

        _mockRoomRepo.Setup(x => x.GetRoomsByRoomsIds(ids)).ReturnsAsync(rooms);

        // Act
        var result = await _roomService.GetRoomsByIds(ids);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(rooms);
    }

    [Fact]
    public async Task GetRooms_ShouldReturnFailure_WhenRoomCategoryNotFound()
    {
        // Arrange
        var hotelId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var sieve = _fixture.Create<SieveModel>();
        var cancellationToken = CancellationToken.None;

        _mockRoomCategoryService.Setup(x =>
            x.GetRoomCategoryById(hotelId, categoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Failure(RoomCategoryError.RoomCategoryNotFound(categoryId)));

        // Act
        var result = await _roomService.GetRooms(hotelId, categoryId, sieve, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task GetRooms_ShouldReturnSuccess_WhenRoomCategoryFound()
    {
        // Arrange
        var hotelId = _fixture.Create<Guid>();
        var categoryId = _fixture.Create<Guid>();
        var rooms = _fixture.Create<List<Room>>();
        var sieve = _fixture.Create<SieveModel>();
        var cancellationToken = CancellationToken.None;

        _mockRoomCategoryService.Setup(x =>
            x.GetRoomCategoryById(hotelId, categoryId, cancellationToken))
            .ReturnsAsync(Result<RoomCategory>.Success(_fixture.Create<RoomCategory>()));

        _mockRoomRepo.Setup(x =>
            x.GetRoomsByRoomCategory(categoryId, sieve, cancellationToken))
            .ReturnsAsync(rooms);

        // Act
        var result = await _roomService.GetRooms(hotelId, categoryId, sieve, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(rooms);
    }
}