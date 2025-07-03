using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Validators;

public class AddCityRequestValidator : AbstractValidator<AddCityRequest>
{
    public AddCityRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("City name is required.")
            .WithErrorCode("InvalidCityName");

        RuleFor(c => c.Country)
            .NotEmpty()
            .WithMessage("Country is required.")
            .WithErrorCode("InvalidCountry");

        RuleFor(c => c.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required.")
            .WithErrorCode("InvalidPostalCode");
    }
}
