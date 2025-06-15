using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers.Extensions;

public static class HotelFeaturedDealMapperExtensions
{
    public static HotelFeaturedDealResponse MapWithDiscount(
        this HotelResponseMapper mapper,
        RoomInfo room)
    {
        var dto = mapper.MapRoomInfoToHotelFeaturedDeal(room);

        dto.CityName = room.Hotel.City?.Name;
        dto.CountryName = room.Hotel.City?.Country;
        
        var discount = room.Discounts.FirstOrDefault();

        if (discount is null) return dto;
        Console.WriteLine($"discount: {discount.DiscountPercentage}");
        Console.WriteLine($"discount: {room.PricePerNight}");
        dto.DiscountStartDate = discount.StartDate;
        dto.DiscountEndDate = discount.EndDate;
        dto.DiscountedPrice = room.PricePerNight * (1 - discount.DiscountPercentage / 100m);

        return dto;
    }
}
