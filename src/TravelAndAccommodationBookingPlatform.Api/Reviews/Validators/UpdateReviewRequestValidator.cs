using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Reviews.Validators;

public class UpdateReviewRequestValidator : AbstractValidator<UpdateReviewRequest>
{
    public UpdateReviewRequestValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty")
            .WithErrorCode("InvalidUserId")
            .WithSeverity(Severity.Error)
            .When(r => r.UserId is not null);
        
        RuleFor(r => r.HotelId)
            .NotEmpty()
            .WithMessage("HotelId cannot be empty")
            .WithErrorCode("InvalidHotelId")
            .WithSeverity(Severity.Error)
            .When(r => r.HotelId is not null);
        
        RuleFor(r => r.Rating)
            .NotEmpty()
            .WithMessage("Rating cannot be empty")
            .InclusiveBetween(1,5)
            .WithMessage("Rating must be between 1 and 5")
            .When(r => r.Rating is not null);
        
        RuleFor(r => r.Content)
            .MaximumLength(500)
            .WithMessage("Content cannot be longer than 500 characters")
            .WithSeverity(Severity.Warning)
            .When(r => r.Content is not null);
    }
}
