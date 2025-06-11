using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos;

namespace TravelAndAccommodationBookingPlatform.Api.Authentication.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(lr => lr.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email is required")
            .WithErrorCode("EMAIL_REQUIRED")
            .WithSeverity(Severity.Error);
        
        RuleFor(lr => lr.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .WithErrorCode("PASSWORD_REQUIRED")
            .WithSeverity(Severity.Error);
    }
}
