using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class ImageError
{
    public static Error ImageNotFound(Guid imageId) => new(
        code: "Image.NotFound",
        message: $"Image with ID '{imageId}' was not found.",
        type: ErrorType.NotFound
    );
}