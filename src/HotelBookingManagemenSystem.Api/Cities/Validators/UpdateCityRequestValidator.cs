using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Validators;

public class UpdateCityRequestValidator : AbstractValidator<UpdateCityRequest>
{
    public UpdateCityRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("City name is required.")
            .WithErrorCode("InvalidCityName")
            .When(c => c.Name is not null);

        RuleFor(c => c.Country)
            .NotEmpty()
            .WithMessage("Country is required.")
            .WithErrorCode("InvalidCountry")
            .When(c => c.Country is not null);

        RuleFor(c => c.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required.")
            .WithErrorCode("InvalidPostalCode")
            .When(c => c.PostalCode is not null);
    }
}
