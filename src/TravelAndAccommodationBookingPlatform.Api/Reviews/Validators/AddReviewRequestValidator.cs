using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Reviews.Validators;

public class AddReviewRequestValidator : AbstractValidator<AddReviewRequest>
{
    public AddReviewRequestValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty")
            .WithErrorCode("InvalidUserId")
            .WithSeverity(Severity.Error);
        
        RuleFor(r => r.Rating)
            .NotEmpty()
            .WithMessage("Rating cannot be empty")
            .InclusiveBetween(1,5)
            .WithMessage("Rating must be between 1 and 5");
        
        RuleFor(r => r.Content)
            .MaximumLength(500)
            .WithMessage("Content cannot be longer than 500 characters")
            .WithSeverity(Severity.Warning);
    }
}
