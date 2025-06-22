using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Emails;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.InvoiceDocuments;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Bookings;

public class BookingService(IBookingRepository bookingRepository,
    IUserService userService,
    IRoomService roomService,
    IEmailService emailService,
    IInvoiceGenerator invoiceGenerator,
    IHotelService hotelService) : IBookingService
{
    public static async Task TestConcurrentBookings(IServiceProvider serviceProvider)
{
    var roomId = Guid.Parse("DEF8DF3C-79A9-44A4-6FE3-08DDAE4D6B86");
    var hotelId = Guid.Parse("EEF1D7A6-0E86-4FB9-5995-08DDACC1DAD4");
    var userId1 = Guid.Parse("fd27468a-0e4c-479f-00a8-08ddaf2b2faa");
    var userId2 = Guid.Parse("24140cd6-a3e7-45db-67b8-08ddae8506d2");

    var booking1 = new Booking {
        HotelId = hotelId,
        UserId = userId1,
        CheckInDate = new DateOnly(2026, 6, 18),
        CheckOutDate = new DateOnly(2026, 7, 18),
        PaymentDetail = new PaymentDetail()
    };

    var booking2 = new Booking {
        HotelId = hotelId,
        UserId = userId2,
        CheckInDate = new DateOnly(2026, 6, 18),
        CheckOutDate = new DateOnly(2026, 7, 18)
    };
    var booking3 = new Booking {
        HotelId = hotelId,
        UserId = userId2,
        CheckInDate = new DateOnly(2026, 6, 18),
        CheckOutDate = new DateOnly(2026, 7, 18)
    };

    // Create separate service scopes for each booking
    using var scope1 = serviceProvider.CreateScope();
    using var scope2 = serviceProvider.CreateScope();
    using var scope3 = serviceProvider.CreateScope();

    var service1 = scope1.ServiceProvider.GetRequiredService<IBookingService>();
    var service2 = scope2.ServiceProvider.GetRequiredService<IBookingService>();
    var service3 = scope3.ServiceProvider.GetRequiredService<IBookingService>();

    var task1 = Task.Run(() => service1.AddBooking(booking1, [roomId]));
    var task2 = Task.Run(() => service2.AddBooking(booking2, [roomId]));
    var task3 = Task.Run(() => service3.AddBooking(booking3, [roomId]));

    try 
    {
        await Task.WhenAll(task1, task2, task3);
        Console.WriteLine("Both bookings succeeded - concurrency issue exists!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Expected failure occurred: {ex.Message}");
        
        // Check database using a NEW context instance
        using var checkScope = serviceProvider.CreateScope();
        var dbContext = checkScope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();
        
        var successfulBookings = await dbContext.Bookings
            .Where(b => b.Rooms.Any(r => r.Id == roomId) &&
                        b.CheckInDate == new DateOnly(2026, 6, 18))
            .ToListAsync();

        Console.WriteLine($"Successful bookings count: {successfulBookings.Count}");
        Console.WriteLine(successfulBookings.Count == 1
            ? "✅ Test passed - only one booking was created"
            : "❌ Test failed - unexpected number of bookings");
    }
}
    
    public async Task<Result<Booking>> AddBooking(Booking booking, List<Guid>? roomsIds)
    {
        ArgumentNullException.ThrowIfNull(booking);

        if (roomsIds is null || roomsIds.Count == 0)
        {
            return Result<Booking>.Failure(BookingError.NoRoomsWithBooking());
        }
        
        var hotelResult = await hotelService.GetHotelById(booking.HotelId);
        if (hotelResult.IsFailure)
        {
            return Result<Booking>.Failure(HotelError.HotelNotFound(booking.HotelId));
        }
        var hotel = hotelResult.Value;
        
        var userResult = await userService.GetUserById(booking.UserId);
        if (userResult.IsFailure)
        {
            return Result<Booking>.Failure(UserError.UserNotFoundById(booking.UserId));
        }
        
        var user = userResult.Value;
        
        var roomsResult = await roomService.GetRoomsByIds(roomsIds);
        if (roomsResult.IsFailure)
        {
            return Result<Booking>.Failure(roomsResult.Error);
        }
        var rooms = roomsResult.Value;
        var result = ValidateRoomAvailability(booking, rooms);
        if (result.IsFailure)
        {
            return Result<Booking>.Failure(result.Error);
        }
        
        booking.BookingDate = DateTime.UtcNow;
        var addResult = await bookingRepository.AddBooking(booking, rooms);
        var addedBooking = addResult.Value;
        
        var invoicePdf = invoiceGenerator.GenerateInvoicePdf(booking);
        
        await emailService.SendConfirmationEmail(user, hotel.Name, addedBooking, invoicePdf);
        return Result<Booking>.Success(addedBooking);
    }
    
    private static Result ValidateRoomAvailability(Booking booking, List<Room> rooms)
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
                return Result.Failure(RoomError.RoomNotAvailable(room.Id));
            }
        }
        return Result.Success();
    }

    private static bool EnsureIfRoomIsBooked(Booking newBooking, Booking oldBooking)
    {
        return newBooking.CheckInDate < oldBooking.CheckOutDate &&
               newBooking.CheckOutDate > oldBooking.CheckInDate;
    }

    public async Task<Result<byte[]>> GenerateInvoiceForBooking(Guid bookingId)
    {
        var booking = await bookingRepository.GetBookingWithDetails(bookingId);
        
        return booking is null ? Result<byte[]>.Failure(BookingError.BookingNotFound(bookingId)) :
            Result<byte[]>.Success(invoiceGenerator.GenerateInvoicePdf(booking));
    }
    
    public async Task UpdateBooking(Booking booking)
    {
        await bookingRepository.UpdateBooking(booking);
    }

    public async Task<Result<Booking>> DeleteBooking(Guid bookingId)
    {
        var result = await GetBookingById(bookingId);
        if (result.IsFailure)
        {
            return Result<Booking>.Failure(BookingError.BookingNotFound(bookingId));
        }
        
        var booking = result.Value;
        await bookingRepository.DeleteBooking(booking);
        return Result<Booking>.Success(booking);
    }

    public async Task<Result<Booking>> GetBookingById(Guid bookingId)
    {
        var booking = await bookingRepository.GetBooking(bookingId);
        return booking is null ? Result<Booking>.Failure(BookingError.BookingNotFound(bookingId)) : Result<Booking>.Success(booking);
    }

    public async Task<Result<Booking>> GetBookingWithDetailsById(Guid bookingId)
    {
        var booking = await bookingRepository.GetBookingWithDetails(bookingId);
        return booking is null ? Result<Booking>.Failure(BookingError.BookingNotFound(bookingId)) : Result<Booking>.Success(booking);
    }

    public async Task<List<Booking>> GetBookings(SieveModel sieveModel)
    {
        return await bookingRepository.GetAllBookings(sieveModel);
    }

    public async Task<Result<List<Booking>>> GetRecentlyVisitedHotels(Guid userId, int listCount,
        CancellationToken cancellationToken = default)
    {
        var userResult = await userService.GetUserById(userId);
        if (userResult.IsFailure)
        {
            return Result<List<Booking>>.Failure(UserError.UserNotFoundById(userId));
        }
        
        var recentlyVisited =  await bookingRepository.GetUserRecentlyVisitedHotels(userId, listCount, cancellationToken);
        return Result<List<Booking>>.Success(recentlyVisited);
    }
}
