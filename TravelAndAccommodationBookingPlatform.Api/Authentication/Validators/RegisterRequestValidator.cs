using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Authentication.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty")
            .MaximumLength(50)
            .WithMessage("First name cannot be more than 50 characters");
        
        RuleFor(r => r.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty")
            .MaximumLength(50)
            .WithMessage("Last name cannot be more than 50 characters");
        
        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address")
            .WithErrorCode("EMAIL_REQUIRED")
            .WithSeverity(Severity.Error);
        
        RuleFor(r => r.Password)
            .NotEmpty()
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()\-_+=\[\]{};:'"",.<>?/\\|`~]).{8,}$")
            .WithMessage("Password must be at least 8 characters long and contain an uppercase letter, a lowercase letter, a number, and a special character.")
            .WithErrorCode("WEAK_PASSWORD")
            .WithSeverity(Severity.Error);
        
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match")
            .WithErrorCode("PASSWORD_MISMATCH");

        RuleFor(r => r.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty");
    }
}
