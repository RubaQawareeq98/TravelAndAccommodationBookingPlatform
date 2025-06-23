using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;

[Mapper]
public partial class HotelResponseMapper
{
    public partial HotelResponse MapHotelToHotelResponse(Hotel hotel);
    public partial List<HotelResponse> MapHotelListToHotelResponseList(List<Hotel> hotels);
    
    [MapProperty(nameof(RoomCategory.Hotel.Id), nameof(HotelFeaturedDealResponse.Id))]
    [MapProperty(nameof(RoomCategory.Hotel.Name), nameof(HotelFeaturedDealResponse.Name))]
    [MapProperty(nameof(RoomCategory.Hotel.Description), nameof(HotelFeaturedDealResponse.Description))]
    [MapProperty(nameof(RoomCategory.Hotel.PhoneNumber), nameof(HotelFeaturedDealResponse.PhoneNumber))]
    [MapProperty(nameof(RoomCategory.Hotel.ThumbnailUrl), nameof(HotelFeaturedDealResponse.ThumbnailUrl))]
    [MapProperty(nameof(RoomCategory.Hotel.StarRating), nameof(HotelFeaturedDealResponse.StarRating))]
    [MapProperty(nameof(RoomCategory.Hotel.TotalRooms), nameof(HotelFeaturedDealResponse.TotalRooms))]
    [MapProperty(nameof(RoomCategory.PricePerNight), nameof(HotelFeaturedDealResponse.OriginalPrice))]
    [MapProperty(nameof(RoomCategory.Hotel.HotelType), nameof(HotelFeaturedDealResponse.HotelType))]
    public partial HotelFeaturedDealResponse MapRoomCategoryToHotelFeaturedDeal(RoomCategory roomCategory);
}