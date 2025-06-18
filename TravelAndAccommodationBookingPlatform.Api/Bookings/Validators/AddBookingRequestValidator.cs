using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Validators;

public class AddBookingRequestValidator : AbstractValidator<AddBookingRequest>
{
    public AddBookingRequestValidator()
    {
        RuleFor(b => b.GuestRemarks)
            .MaximumLength(500)
            .WithMessage("Description must be less than 400 characters")
            .WithErrorCode("InvalidDescription")
            .WithSeverity(Severity.Warning);
        
        RuleFor(b => b.HotelId)
            .NotEmpty()
            .WithMessage("Hotel Id is required")
            .WithErrorCode("InvalidHotelId")
            .WithSeverity(Severity.Error);
        
        RuleFor(b => b.CheckInDate)
            .NotEmpty()
            .WithMessage("Check-in Date is required")
            .WithErrorCode("InvalidCheckInDate")
            .WithSeverity(Severity.Error);
        
        RuleFor(b => b.CheckOutDate)
            .NotEmpty()
            .WithMessage("Check-out Date is required")
            .WithErrorCode("InvalidCheckOutDate")
            .WithSeverity(Severity.Error);
        
        RuleFor(b => b.BookingDate)
            .NotEmpty()
            .WithMessage("Booking Date is required")
            .WithErrorCode("InvalidBookingDate")
            .WithSeverity(Severity.Error);

        RuleFor(b => b.RoomsIds)
            .NotNull()
            .WithMessage("You must provide at least one room");
    }
}
