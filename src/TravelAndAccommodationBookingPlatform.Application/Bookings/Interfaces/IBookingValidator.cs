using TravelAndAccommodationBookingPlatform.Application.Bookings.Dtos;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Application.Bookings.Interfaces;

public interface IBookingValidator
{
    Task<Result<BookingValidationResult>> ValidateBooking(Guid userId, Booking booking, List<Guid>? roomIds);
}
