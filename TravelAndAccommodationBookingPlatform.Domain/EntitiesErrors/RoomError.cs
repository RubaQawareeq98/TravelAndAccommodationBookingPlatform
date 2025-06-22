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
    
    public static Error RoomNumberAlreadyExist(string roomNumber) => new(
        code: "room.AlreadyExist",
        message: $"room with Number '{roomNumber}' already exist in this category.",
        type: ErrorType.Conflict
    );
    
    
}
