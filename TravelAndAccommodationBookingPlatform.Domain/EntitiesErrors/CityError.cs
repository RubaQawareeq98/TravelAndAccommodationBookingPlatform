using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class CityError
{
    public static Error NotFound(Guid cityId) => new(
        code: "City.NotFound",
        message: $"City with ID '{cityId}' was not found.",
        type: ErrorType.NotFound
    );
    
    public static Error AlreadyExists(string cityName) => new(
        code: "City.AlreadyExists",
        message: $"City with name '{cityName}' already exists.",
        type: ErrorType.Conflict
    );
}
