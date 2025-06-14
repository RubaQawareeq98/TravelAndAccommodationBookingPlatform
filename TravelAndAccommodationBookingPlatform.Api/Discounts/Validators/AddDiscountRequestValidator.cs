using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Discounts.Validators;

public class AddDiscountRequestValidator : AbstractValidator<AddDiscountRequest>
{
    public AddDiscountRequestValidator()
    {
        RuleFor(d => d.DiscountPercentage)
            .NotEmpty()
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Discount percentage must be between 0 and 100.")
            .WithErrorCode("InvalidPercentage");

        RuleFor(d => d.RoomInfoId)
            .NotEmpty()
            .WithMessage("Room id cannot be empty.")
            .WithErrorCode("InvalidRoomId");
        
        RuleFor(d => d.StartDate)
            .NotEmpty()
            .GreaterThan(DateTime.Now)
            .WithMessage("Start date cannot be empty.")
            .WithErrorCode("InvalidStartDate");
        
        RuleFor(d => d.EndDate)
            .NotEmpty()
            .GreaterThan(d => d.StartDate)
            .WithMessage("End date cannot be empty.")
            .WithErrorCode("InvalidStartDate");
    }
}
