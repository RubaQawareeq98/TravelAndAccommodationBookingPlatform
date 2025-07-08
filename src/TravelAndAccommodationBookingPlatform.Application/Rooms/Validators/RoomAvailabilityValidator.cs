using TravelAndAccommodationBookingPlatform.Application.Rooms.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Application.Rooms.Validators;

public class RoomAvailabilityValidator : IRoomAvailabilityValidator
{
    public Result ValidateRoomAvailability(Booking booking, List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            if (room.RoomCategory.HotelId != booking.HotelId)
            {
                return Result.Failure(RoomCategoryError.RoomCategoryNotBelongToHotel(room.RoomCategoryId, booking.HotelId));
            }

            var isRoomBooked = room.Bookings.Any(b =>
                EnsureIfRoomIsBooked(booking, b)
            );
            
            if (isRoomBooked)
            {
                return Result<Room>.Failure(RoomError.RoomNotAvailable(room.Id));
            }
        }
        return Result.Success();
    }

    private static bool EnsureIfRoomIsBooked(Booking newBooking, Booking oldBooking)
    {
        return newBooking.CheckInDate < oldBooking.CheckOutDate &&
               newBooking.CheckOutDate > oldBooking.CheckInDate;
    }
}
