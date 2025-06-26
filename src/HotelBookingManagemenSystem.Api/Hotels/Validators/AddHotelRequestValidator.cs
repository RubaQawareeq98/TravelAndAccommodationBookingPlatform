using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Validators;

public class AddHotelRequestValidator : AbstractValidator<AddHotelRequest>
{
    public AddHotelRequestValidator()
    {
        RuleFor(h => h.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .WithErrorCode("InvalidName");
        
        RuleFor(h => h.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .WithErrorCode("InvalidPhoneNumber");
        
        RuleFor(h => h.StarRating)
            .InclusiveBetween(1, 5)
            .WithMessage("StarRating must be between 1 and 5.")
            .WithErrorCode("InvalidStarRating");

        RuleFor(h => h.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.")
            .WithErrorCode("InvalidLongitude");

        RuleFor(h => h.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.")
            .WithErrorCode("InvalidLatitude");

        RuleFor(h => h.TotalRooms)
            .GreaterThan(0)
            .WithMessage("TotalRooms must be greater than 0.")
            .WithErrorCode("InvalidTotalRooms");

        RuleFor(h => h.HotelType)
            .IsInEnum()
            .WithMessage("HotelType must be a valid value.")
            .WithErrorCode("InvalidHotelType");

        RuleFor(h => h.CityId)
            .NotEmpty()
            .WithMessage("CityId is required.")
            .WithErrorCode("InvalidCityId");

        RuleFor(h => h.OwnerId)
            .NotEmpty()
            .WithMessage("OwnerId is required.")
            .WithErrorCode("InvalidOwnerId");
    }
}
