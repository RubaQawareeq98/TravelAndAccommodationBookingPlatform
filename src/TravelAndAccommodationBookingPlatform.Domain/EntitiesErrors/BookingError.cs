using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;

public abstract class BookingError
{
    public static Error BookingNotFound(Guid bookingId) => new(
        code: "Booking.NotFound",
        message: $"Booking with ID '{bookingId}' was not found.",
        type: ErrorType.NotFound
    );
    
    public static Error NoRoomsWithBooking() => new(
        code: "Booking.BadRequest",
        message: "You must provide at least one room with a booking.",
        type: ErrorType.BadRequest
    );
    
    public static Error BookingCancelError() => new(
        code: "Booking.Forbidden",
        message: "Can't cancel an old booking.",
        type: ErrorType.Forbidden
    );
}
