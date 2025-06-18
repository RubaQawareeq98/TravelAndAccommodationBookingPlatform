using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Bookings;

public class BookingService(IBookingRepository bookingRepository,
    IUserService userService,
    IRoomService roomService,
    IHotelService hotelService) : IBookingService
{
    public async Task AddBooking(Booking booking, List<Guid> roomsIds)
    {
        await hotelService.GetHotelById(booking.HotelId);
        await userService.GetUserByIdAsync(booking.UserId);

        await ValidateRoomAvailability(booking, roomsIds);
        
        await bookingRepository.AddBooking(booking);
    }

    private async Task ValidateRoomAvailability(Booking booking, List<Guid> roomIds)
    {
        var rooms = await roomService.GetRoomsByIds(roomIds);

        if (rooms.Count != roomIds.Count)
        {
            throw new NotFoundException("One or more rooms do not exist.");
        }

        foreach (var room in rooms)
        {
            if (room.RoomInfo.HotelId != booking.HotelId)
            {
                throw new InvalidOperationException($"Room with id: {room.RoomInfo.Id} does not belong to the selected hotel.");
            }

            var isRoomBooked = room.Bookings.Any(b =>
                EnsureIfRoomIsBooked(booking, b)
            );

            if (isRoomBooked)
            {
                throw new InvalidOperationException($"Room with id: {room.RoomInfo.Id} is not available for the selected date.");
            }
            
            booking.Rooms.Add(room);
        }
    }

    private static bool EnsureIfRoomIsBooked(Booking newBooking, Booking oldBooking)
    {
        return newBooking.CheckInDate < oldBooking.CheckOutDate &&
               newBooking.CheckOutDate > oldBooking.CheckInDate;
    }

    public async Task UpdateBooking(Booking booking)
    {
        await bookingRepository.UpdateBooking(booking);
    }

    public async Task DeleteBooking(Guid bookingId)
    {
        var booking = await GetBookingById(bookingId);
        await bookingRepository.DeleteBooking(booking);
    }

    public async Task<Booking> GetBookingById(Guid bookingId)
    {
        var booking = await bookingRepository.GetBooking(bookingId);
        if (booking is null)
        {
            throw new NotFoundException($"Booking with id {bookingId} not found");
        }
        
        return booking;
    }

    public async Task<List<Booking>> GetBookings(SieveModel sieveModel)
    {
        return await bookingRepository.GetAllBookings(sieveModel);
    }

    public async Task<List<Booking>> GetRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default)
    {
        var user = await userService.GetUserByIdAsync(userId);
        
        return await bookingRepository.GetUserRecentlyVisitedHotels(user.Id, listCount, cancellationToken);
    }
}

