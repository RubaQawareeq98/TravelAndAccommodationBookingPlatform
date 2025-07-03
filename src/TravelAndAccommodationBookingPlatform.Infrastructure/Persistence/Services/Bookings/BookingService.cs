using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Application.Bookings.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Emails.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.InvoiceDocuments.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Payments.Dtos;
using TravelAndAccommodationBookingPlatform.Application.Payments.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Rooms.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Bookings;

public class BookingService(IBookingRepository bookingRepository,
    IUserService userService,
    IEmailService emailService,
    IInvoiceGenerator invoiceGenerator,
    IPaymentService paymentService,
    IBookingValidator bookingValidator,
    IRoomAvailabilityValidator roomAvailabilityValidator) : IBookingService
{
    public async Task<Result<Booking>> AddBooking(Guid userId,
        Booking booking, List<Guid>? roomIds,
        CancellationToken cancellationToken = default)
    {
        
        
        var validationResult = await bookingValidator.ValidateBooking(userId, booking, roomIds);
        if (validationResult.IsFailure)
        {
            return Result<Booking>.Failure(validationResult.Error);
        }
        var rooms = validationResult.Value.Rooms;
        var user = validationResult.Value.User;
        var hotel = validationResult.Value.Hotel;
        
        var result = roomAvailabilityValidator.ValidateRoomAvailability(booking, rooms);
        if (result.IsFailure)
        {
            return Result<Booking>.Failure(result.Error);
        }
        
        booking.UserId = userId;
        booking.BookingDate = DateTime.UtcNow;
        var addResult = await bookingRepository.AddBooking(booking, rooms, cancellationToken);
        var addedBooking = addResult.Value;
        
        var invoicePdf = invoiceGenerator.GenerateInvoicePdf(booking);
        
        await emailService.SendConfirmationEmail(user, hotel.Name, addedBooking, invoicePdf);
        
        if (booking.PaymentDetail.PaymentMethod == PaymentMethod.Cash) return Result<Booking>.Success(addedBooking);
        var paymentRequest = new AddPaymentRequest
        {
            Amount = addedBooking.PaymentDetail.Amount,
            ReceiptEmail = user.Email
        };
            
        var paymentResult = await paymentService.CreatePaymentService(paymentRequest);
        return paymentResult.IsFailure ? Result<Booking>.Failure(paymentResult.Error) : Result<Booking>.Success(addedBooking);
    }

    public async Task<Result<byte[]>> GenerateInvoiceForBooking(Guid userId, Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.GetBookingWithDetails(userId, bookingId, cancellationToken);
        
        return booking is null ? Result<byte[]>.Failure(BookingError.BookingNotFound(bookingId)) :
            Result<byte[]>.Success(invoiceGenerator.GenerateInvoicePdf(booking));
    }
    
    public async Task UpdateBooking(Booking booking)
    {
        await bookingRepository.UpdateBooking(booking);
    }

    public async Task<Result<Booking>> DeleteBooking(Guid userId, Guid bookingId, CancellationToken cancellationToken = default)
    {
        var result = await GetBookingById(userId, bookingId, cancellationToken);
        if (result.IsFailure)
        {
            return Result<Booking>.Failure(BookingError.BookingNotFound(bookingId));
        }
        
        var booking = result.Value;
        await bookingRepository.DeleteBooking(booking);
        return Result<Booking>.Success(booking);
    }

    public async Task<Result<Booking>> GetBookingById(Guid userId, Guid bookingId, CancellationToken cancellationToken = default)
    {
        var userResult = await userService.GetUserById(userId);
        if (userResult.IsFailure)
        {
            return Result<Booking>.Failure(userResult.Error);
        }
        
        var booking = await bookingRepository.GetBooking(userId, bookingId, cancellationToken);
        return booking is null ? Result<Booking>.Failure(BookingError.BookingNotFound(bookingId)) : Result<Booking>.Success(booking);
    }

    public async Task<Result<Booking>> GetBookingWithDetailsById(Guid userId, Guid bookingId, CancellationToken cancellationToken = default)
    {
        var userResult = await userService.GetUserById(userId);
        if (userResult.IsFailure)
        {
            return Result<Booking>.Failure(userResult.Error);
        }
        
        var booking = await bookingRepository.GetBookingWithDetails(userId, bookingId, cancellationToken);
        return booking is null ? Result<Booking>.Failure(BookingError.BookingNotFound(bookingId)) : Result<Booking>.Success(booking);
    }

    public async Task<Result<List<Booking>>> GetBookings(SieveModel sieveModel, Guid userId,
        CancellationToken cancellationToken)
    {
        var userResult = await userService.GetUserById(userId);
        if (userResult.IsFailure)
        {
            return Result<List<Booking>>.Failure(userResult.Error);
        }
        
        var bookings =  await bookingRepository.GetAllBookings(sieveModel, userId, cancellationToken);
        return Result<List<Booking>>.Success(bookings);
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

/*

    public static async Task TestConcurrentBookings(IServiceProvider serviceProvider)
{
    var roomId = Guid.Parse("DEF8DF3C-79A9-44A4-6FE3-08DDAE4D6B86");
    var hotelId = Guid.Parse("EEF1D7A6-0E86-4FB9-5995-08DDACC1DAD4");
    var userId1 = Guid.Parse("fd27468a-0e4c-479f-00a8-08ddaf2b2faa");
    var userId2 = Guid.Parse("24140cd6-a3e7-45db-67b8-08ddae8506d2");

    var booking1 = new Booking {
        HotelId = hotelId,
        UserId = userId1,
        CheckInDate = new DateOnly(2023, 6, 18),
        CheckOutDate = new DateOnly(2023, 7, 18),
        PaymentDetail = new PaymentDetail()
    };

    var booking2 = new Booking {
        HotelId = hotelId,
        UserId = userId2,
        CheckInDate = new DateOnly(2023, 6, 18),
        CheckOutDate = new DateOnly(2023, 7, 18)
    };
    var booking3 = new Booking {
        HotelId = hotelId,
        UserId = userId2,
        CheckInDate = new DateOnly(2023, 6, 18),
        CheckOutDate = new DateOnly(2023, 7, 18)
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
        
        using var checkScope = serviceProvider.CreateScope();
        var dbContext = checkScope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();
        
        var successfulBookings = await dbContext.Bookings
            .Where(b => b.Rooms.Any(r => r.Id == roomId) &&
                        b.CheckInDate == new DateOnly(2026, 6, 18))
            .ToListAsync();

        Console.WriteLine($"Successful bookings count: {successfulBookings.Count}");
        Console.WriteLine(successfulBookings.Count == 1
            ? " Test passed - only one booking was created"
            : "Test failed - unexpected number of bookings");
    }
}


*/