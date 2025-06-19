using FluentValidation;
using TravelAndAccommodationBookingPlatform.Api.Amenities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Amenities.Validators;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Validators;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Validators;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Cities.Validators;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Validators;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Validators;
using TravelAndAccommodationBookingPlatform.Api.Images.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Images.Validators;
using TravelAndAccommodationBookingPlatform.Api.Owners.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Owners.Validators;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Validators;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Validators;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Validators;
using TravelAndAccommodationBookingPlatform.Api.Users.Dtos;
using TravelAndAccommodationBookingPlatform.Api.Users.Validators;

namespace TravelAndAccommodationBookingPlatform.Api.Configurations;

public static class ValidatorsConfiguration
{
    public static IServiceCollection AddValidatorsConfigurations(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddSingleton<IValidator<RegisterRequest>, RegisterRequestValidator>();
        services.AddSingleton<IValidator<AddCityRequest>, AddCityRequestValidator>();
        services.AddSingleton<IValidator<AddBookingRequest>, AddBookingRequestValidator>();
        services.AddSingleton<IValidator<AddOwnerRequest>, AddOwnerRequestValidator>();
        services.AddSingleton<IValidator<UpdateOwnerRequest>, UpdateOwnerRequestValidator>();
        services.AddSingleton<IValidator<UpdateCityRequest>, UpdateCityRequestValidator>();
        services.AddSingleton<IValidator<ImageUploadRequest>, ThumbnailImageUploadRequestValidator>();
        services.AddSingleton<IValidator<AddRoomInfoRequest>, AddRoomInfoRequestValidator>();
        services.AddSingleton<IValidator<AddRoomRequest>, AddRoomRequestValidator>();
        services.AddSingleton<IValidator<AddAmenityRequest>, AddAmenityRequestValidator>();
        services.AddSingleton<IValidator<AddHotelRequest>, AddHotelRequestValidator>();
        services.AddSingleton<IValidator<AddReviewRequest>, AddReviewRequestValidator>();
        services.AddSingleton<IValidator<AddDiscountRequest>, AddDiscountRequestValidator>();
        services.AddSingleton<IValidator<GetFeaturedDealsRequest>, GetFeaturedDealsRequestValidator>();
        services.AddSingleton<IValidator<GetRecentlyVisitedRequest>, GetRecentlyVisitedRequestValidator>();

        return services;
    }
}
