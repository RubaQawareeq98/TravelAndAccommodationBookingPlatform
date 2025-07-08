using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Requests;

namespace TravelAndAccommodationBookingPlatform.Api.RoomCategories.Validators;

public class AddRoomCategoryRequestValidator : AbstractValidator<AddRoomCategoryRequest>
{
    public AddRoomCategoryRequestValidator()
    {
        RuleFor(ri => ri.ChildrenCapacity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Room Capacity must be greater than 0")
            .WithErrorCode("InvalidCapacity");
        
        RuleFor(ri => ri.AdultsCapacity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Room Capacity must be greater than 0")
            .WithErrorCode("InvalidCapacity");
        
        RuleFor(ri => ri.RoomType)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Room Type is required")
            .WithErrorCode("InvalidRoomType");

        RuleFor(ri => ri.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(ri => ri.Description)
            .MaximumLength(400)
            .WithMessage("Description must be less than 400 characters")
            .WithErrorCode("InvalidDescription")
            .WithSeverity(Severity.Warning);

        RuleFor(ri => ri.PricePerNight)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Price Per Night must be greater than 0");
    }
}
