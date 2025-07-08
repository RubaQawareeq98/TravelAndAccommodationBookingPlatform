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

public class BookingService(
    IBookingRepository bookingRepository,
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
        var addedBooking = await bookingRepository.AddBooking(booking, rooms, cancellationToken);

        var invoicePdf = invoiceGenerator.GenerateInvoicePdf(addedBooking);

        await emailService.SendConfirmationEmail(user, hotel.Name, addedBooking, invoicePdf);

        if (booking.PaymentDetails.PaymentMethod == PaymentMethod.Cash) return Result<Booking>.Success(addedBooking);
        var paymentRequest = new AddPaymentRequest
        {
            Amount = addedBooking.PaymentDetails.Amount,
            ReceiptEmail = user.Email
        };

        var paymentResult = await paymentService.CreatePaymentService(paymentRequest);
        return paymentResult.IsFailure
            ? Result<Booking>.Failure(paymentResult.Error)
            : Result<Booking>.Success(addedBooking);
    }

    public async Task<Result<byte[]>> GenerateInvoiceForBooking(Guid userId, Guid bookingId,
        CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.GetBookingWithDetails(userId, bookingId, cancellationToken);

        return booking is null
            ? Result<byte[]>.Failure(BookingError.BookingNotFound(bookingId))
            : Result<byte[]>.Success(invoiceGenerator.GenerateInvoicePdf(booking));
    }

    public async Task UpdateBooking(Booking booking)
    {
        await bookingRepository.UpdateBooking(booking);
    }

    public async Task<Result> DeleteBooking(Guid userId, Guid bookingId, CancellationToken cancellationToken = default)
    {
        var result = await GetBookingById(userId, bookingId, cancellationToken);
        if (result.IsFailure)
        {
            return Result.Failure(BookingError.BookingNotFound(bookingId));
        }

        var booking = result.Value;
        if (booking.CheckInDate <= DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return Result.Failure(BookingError.BookingCancelError());
        }

        await bookingRepository.DeleteBooking(booking);
        return Result.Success();
    }

    public async Task<Result<Booking>> GetBookingById(Guid userId, Guid bookingId,
        CancellationToken cancellationToken = default)
    {
        var userResult = await userService.GetUserById(userId);
        if (userResult.IsFailure)
        {
            return Result<Booking>.Failure(userResult.Error);
        }

        var booking = await bookingRepository.GetBooking(userId, bookingId, cancellationToken);
        return booking is null
            ? Result<Booking>.Failure(BookingError.BookingNotFound(bookingId))
            : Result<Booking>.Success(booking);
    }

    public async Task<Result<Booking>> GetBookingWithDetailsById(Guid userId, Guid bookingId,
        CancellationToken cancellationToken = default)
    {
        var userResult = await userService.GetUserById(userId);
        if (userResult.IsFailure)
        {
            return Result<Booking>.Failure(userResult.Error);
        }

        var booking = await bookingRepository.GetBookingWithDetails(userId, bookingId, cancellationToken);
        return booking is null
            ? Result<Booking>.Failure(BookingError.BookingNotFound(bookingId))
            : Result<Booking>.Success(booking);
    }

    public async Task<Result<List<Booking>>> GetBookings(SieveModel sieveModel, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userResult = await userService.GetUserById(userId);
        if (userResult.IsFailure)
        {
            return Result<List<Booking>>.Failure(userResult.Error);
        }

        var bookings = await bookingRepository.GetUserBookings(sieveModel, userId, cancellationToken);
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

        var recentlyVisited = await bookingRepository.GetUserRecentlyVisitedHotels(userId, listCount, cancellationToken);
        return Result<List<Booking>>.Success(recentlyVisited);
    }
}
