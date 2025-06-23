using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.RoomCategories.Validators;

public class UpdateRoomCategoryRequestValidator : AbstractValidator<UpdateRoomCategoryRequest>
{
    public UpdateRoomCategoryRequestValidator()
    {
        RuleFor(ri => ri.ChildrenCapacity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Room Capacity must be greater than 0")
            .WithErrorCode("InvalidCapacity")
            .When(ri => ri.ChildrenCapacity is not null);

        RuleFor(ri => ri.AdultsCapacity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Room Capacity must be greater than 0")
            .WithErrorCode("InvalidCapacity")
            .When(ri => ri.AdultsCapacity is not null);

        RuleFor(ri => ri.RoomType)
            .IsInEnum()
            .WithMessage("Room Type is required")
            .WithErrorCode("InvalidRoomType")
            .When(ri => ri.RoomType is not null);

        RuleFor(ri => ri.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .WithErrorCode("InvalidName")
            .When(ri => ri.Name is not null);
        
        RuleFor(ri => ri.Description)
            .MaximumLength(400)
            .WithMessage("Description must be less than 400 characters")
            .WithErrorCode("InvalidDescription")
            .WithSeverity(Severity.Warning)
            .When(ri => ri.Description is not null);
        
        RuleFor(ri => ri.PricePerNight)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Price Per Night must be greater than 0")
            .WithErrorCode("InvalidPricePerNight")
            .When(ri => ri.PricePerNight is not null);
    }
}
