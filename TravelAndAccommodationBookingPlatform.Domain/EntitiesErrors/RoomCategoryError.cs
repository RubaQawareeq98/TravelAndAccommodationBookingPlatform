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
}
