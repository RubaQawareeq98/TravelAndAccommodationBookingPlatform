using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Amenities.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Amenities.Validators;

public class AddAmenityRequestValidator : AbstractValidator<AddAmenityRequest>
{
    public AddAmenityRequestValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(50)
            .WithErrorCode("InvalidName");

        RuleFor(a => a.Description)
            .MaximumLength(500);
    }
}
