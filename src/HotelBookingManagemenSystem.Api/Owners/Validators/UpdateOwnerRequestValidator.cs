using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Owners.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Owners.Validators;

public class UpdateOwnerRequestValidator : AbstractValidator<UpdateOwnerRequest>
{
    public UpdateOwnerRequestValidator()
    {
        RuleFor(o => o.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty")
            .WithErrorCode("FirstName")
            .MaximumLength(50)
            .WithMessage("First name cannot be more than 50 characters")
            .When(o => o.FirstName is not null);
        
        RuleFor(o => o.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty")
            .WithErrorCode("LastName")
            .MaximumLength(50)
            .WithMessage("Last name cannot be more than 50 characters")
            .When(o => o.LastName is not null);

        RuleFor(o => o.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email address cannot be empty")
            .WithErrorCode("Email")
            .When(o => o.Email is not null);
        
        RuleFor(o => o.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty")
            .WithErrorCode("PhoneNumber")
            .When(o => o.PhoneNumber is not null);
    }
}
