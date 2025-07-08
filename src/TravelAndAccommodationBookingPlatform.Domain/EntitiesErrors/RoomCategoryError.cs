using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class RoomCategoryError
{
    public static Error RoomCategoryNotFound(Guid roomCategoryId) => new(
        code: "RoomCategory.NotFound",
        message: $"RoomCategory with ID '{roomCategoryId}' was not found.",
        type: ErrorType.NotFound
    );
    
    public static Error RoomCategoryNotBelongToHotel(Guid roomCategoryId, Guid hotelId) => new(
        code: "RoomCategory.NotFound",
        message: $"RoomCategory with ID '{roomCategoryId}' was not found in hotel with ID '{hotelId}'.",
        type: ErrorType.BadRequest
    );
}
