using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class RoomError
{
    public static Error RoomNotFound(Guid roomId) => new(
        code: "room.NotFound",
        message: $"room with ID '{roomId}' was not found.",
        type: ErrorType.NotFound
    );
    
    public static Error NoRoomsFound() => new(
        code: "room.NotFound",
        message: "Invalid Room ID.",
        type: ErrorType.NotFound
    );
    
    public static Error RoomNumberAlreadyExist(string roomNumber) => new(
        code: "room.AlreadyExist",
        message: $"room with Number '{roomNumber}' already exist in this category.",
        type: ErrorType.Conflict
    );
      
    public static Error RoomNotAvailable(Guid roomId) => new(
        code: "room.NotAvailable",
        message: $"Room with id: {roomId} is not available for the selected date.",
        type: ErrorType.Conflict
    );
}
