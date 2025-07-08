using TravelAndAccommodationBookingPlatform.Application.Bookings.Dtos;
using TravelAndAccommodationBookingPlatform.Application.Bookings.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Application.Bookings.Validators;

public class BookingValidator( IUserService userService,
    IRoomService roomService,
    IHotelService hotelService) : IBookingValidator
{
    public async Task<Result<BookingValidationResult>> ValidateBooking(Guid userId, Booking booking, List<Guid>? roomIds)
    {
        ArgumentNullException.ThrowIfNull(booking);

        if (roomIds is null || roomIds.Count == 0)
        {
            return Result<BookingValidationResult>.Failure(BookingError.NoRoomsWithBooking());
        }
        
        var hotelResult = await hotelService.GetHotelById(booking.HotelId);
        if (hotelResult.IsFailure)
        {
            return Result<BookingValidationResult>.Failure(HotelError.HotelNotFound(booking.HotelId));
        }
        
        var userResult = await userService.GetUserById(userId);
        if (userResult.IsFailure)
        {
            return Result<BookingValidationResult>.Failure(UserError.UserNotFoundById(booking.UserId));
        }
        
        var roomsResult = await roomService.GetRoomsByIds(roomIds);
        if (roomsResult.IsFailure)
        {
            return Result<BookingValidationResult>.Failure(roomsResult.Error);
        }

        return Result<BookingValidationResult>.Success(new BookingValidationResult()
        {
            User = userResult.Value,
            Hotel = hotelResult.Value,
            Rooms = roomsResult.Value
        });
    }
}
