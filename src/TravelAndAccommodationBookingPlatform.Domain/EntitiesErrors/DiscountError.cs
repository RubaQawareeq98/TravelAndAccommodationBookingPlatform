using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class DiscountError
{
    public static Error DiscountNotFound(Guid discountId) => new(
        code: "discount.NotFound",
        message: $"discount with ID '{discountId}' was not found.",
        type: ErrorType.NotFound
    );
}