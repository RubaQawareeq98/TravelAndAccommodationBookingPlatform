using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Validators;

public class UpdateHotelRequestValidator : AbstractValidator<UpdateHotelRequest>
{
    public UpdateHotelRequestValidator()
    {
        RuleFor(h => h.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .WithErrorCode("InvalidName")
            .When(h => h.Name is not null);

        RuleFor(h => h.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .WithErrorCode("InvalidPhoneNumber")
            .When(h => h.PhoneNumber is not null);

        RuleFor(h => h.StarRating)
            .InclusiveBetween(1, 5)
            .WithMessage("StarRating must be between 1 and 5.")
            .WithErrorCode("InvalidStarRating")
            .When(h => h.StarRating.HasValue);

        RuleFor(h => h.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.")
            .WithErrorCode("InvalidLongitude")
            .When(h => h.Longitude.HasValue);

        RuleFor(h => h.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.")
            .WithErrorCode("InvalidLatitude")
            .When(h => h.Latitude.HasValue);

        RuleFor(h => h.TotalRooms)
            .GreaterThan(0)
            .WithMessage("TotalRooms must be greater than 0.")
            .WithErrorCode("InvalidTotalRooms")
            .When(h => h.TotalRooms.HasValue);

        RuleFor(h => h.HotelType)
            .IsInEnum()
            .WithMessage("HotelType must be a valid value.")
            .WithErrorCode("InvalidHotelType")
            .When(h => h.HotelType.HasValue);

        RuleFor(h => h.CityId)
            .NotEmpty()
            .WithMessage("CityId is required.")
            .WithErrorCode("InvalidCityId")
            .When(h => h.CityId.HasValue);

        RuleFor(h => h.OwnerId)
            .NotEmpty()
            .WithMessage("OwnerId is required.")
            .WithErrorCode("InvalidOwnerId")
            .When(h => h.OwnerId.HasValue);
    }
}
