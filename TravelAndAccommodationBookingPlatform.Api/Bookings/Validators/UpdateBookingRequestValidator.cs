using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Validators;

public class UpdateBookingRequestValidator : AbstractValidator<UpdateBookingRequest>
{
    public UpdateBookingRequestValidator()
    {
        RuleFor(b => b.GuestRemarks)
            .MaximumLength(500)
            .WithMessage("Description must be less than 400 characters")
            .WithErrorCode("InvalidDescription")
            .WithSeverity(Severity.Warning)
            .When(b => b.GuestRemarks is not null);
        
        RuleFor(b => b.HotelId)
            .NotEmpty()
            .WithMessage("Hotel Id is required")
            .WithErrorCode("InvalidHotelId")
            .WithSeverity(Severity.Error)
            .When(b => b.HotelId is not null);
        
        RuleFor(b => b.CheckInDate)
            .NotEmpty()
            .WithMessage("Check-in Date is required")
            .WithErrorCode("InvalidCheckInDate")
            .WithSeverity(Severity.Error)
            .When(b => b.CheckInDate is not null);
        
        RuleFor(b => b.CheckOutDate)
            .NotEmpty()
            .WithMessage("Check-out Date is required")
            .WithErrorCode("InvalidCheckOutDate")
            .WithSeverity(Severity.Error)
            .When(b => b.CheckOutDate is not null);
        
        RuleFor(b => b.BookingDate)
            .NotEmpty()
            .WithMessage("Booking Date is required")
            .WithErrorCode("InvalidBookingDate")
            .WithSeverity(Severity.Error)
            .When(b => b.BookingDate is not null);
    }
}
