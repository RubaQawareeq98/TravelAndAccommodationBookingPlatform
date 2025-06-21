using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class UserError
{
    public static Error UserNotFoundById(Guid userId) => new(
        code: "User.NotFound",
        message: $"User with ID '{userId}' was not found.",
        type: ErrorType.NotFound
    );
    
    public static Error EmailAlreadyUsed(string email) => new(
        code: "User.EmailAlreadyUsed",
        message: $"User with email '{email}' is already used.",
        type: ErrorType.Conflict
    );

    public static Error UserUnauthorized() => new(
        code: "User.NotFound",
        message: "User not authorized to access the resource.",
        type: ErrorType.Unauthorized
    );
    
    public static Error MisMatchedPassword() => new(
        code: "User.MismatchedPassword",
        message: "Password does not match.",
        type: ErrorType.BadRequest
    );
}
