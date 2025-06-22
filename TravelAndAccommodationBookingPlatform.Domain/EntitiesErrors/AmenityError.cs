using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class AmenityError
{
    public static Error AmenityNotFound(Guid amenityId) => new(
        code: "Amenity.NotFound",
        message: $"Amenity with ID '{amenityId}' was not found.",
        type: ErrorType.NotFound
    );
    
    public static Error AmenityNameAlreadyExists(string amenityName) => new(
        code: "Amenity.AlreadyExists",
        message: $"Amenity with name '{amenityName}' already exists.",
        type: ErrorType.Conflict
    );
}
