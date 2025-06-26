using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Owners.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Owners.Validators;

public class AddOwnerRequestValidator : AbstractValidator<AddOwnerRequest>
{
    public AddOwnerRequestValidator()
    {
        RuleFor(o => o.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty")
            .WithErrorCode("FirstName")
            .MaximumLength(50)
            .WithMessage("First name cannot be more than 50 characters");
        
        RuleFor(o => o.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty")
            .WithErrorCode("LastName")
            .MaximumLength(50)
            .WithMessage("Last name cannot be more than 50 characters");

        RuleFor(o => o.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email address cannot be empty")
            .WithErrorCode("Email");
        
        RuleFor(o => o.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty")
            .WithErrorCode("PhoneNumber");
    }
}
