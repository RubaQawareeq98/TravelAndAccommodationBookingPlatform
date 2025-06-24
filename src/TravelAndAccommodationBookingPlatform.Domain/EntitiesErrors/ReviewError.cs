using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class ReviewError
{
    public static Error ReviewNotFound(Guid reviewId) => new(
        code: "Review.NotFound",
        message: $"Review with ID '{reviewId}' was not found.",
        type: ErrorType.NotFound
    );
}