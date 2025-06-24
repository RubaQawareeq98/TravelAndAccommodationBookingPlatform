using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class HotelError
{
    public static Error HotelNotFound(Guid hotelId) => new(
        code: "Hotel.NotFound",
        message: $"Hotel with ID '{hotelId}' was not found.",
        type: ErrorType.NotFound
    );
}
