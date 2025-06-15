using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;

[Mapper]
public partial class HotelResponseMapper
{
    public partial HotelResponse MapHotelToHotelResponse(Hotel hotel);
    public partial List<HotelResponse> MapHotelListToHotelResponseList(List<Hotel> hotels);
    
    [MapProperty(nameof(RoomInfo.Hotel.Id), nameof(HotelFeaturedDealResponse.Id))]
    [MapProperty(nameof(RoomInfo.Hotel.Name), nameof(HotelFeaturedDealResponse.Name))]
    [MapProperty(nameof(RoomInfo.Hotel.Description), nameof(HotelFeaturedDealResponse.Description))]
    [MapProperty(nameof(RoomInfo.Hotel.PhoneNumber), nameof(HotelFeaturedDealResponse.PhoneNumber))]
    [MapProperty(nameof(RoomInfo.Hotel.ThumbnailUrl), nameof(HotelFeaturedDealResponse.ThumbnailUrl))]
    [MapProperty(nameof(RoomInfo.Hotel.StarRating), nameof(HotelFeaturedDealResponse.StarRating))]
    [MapProperty(nameof(RoomInfo.Hotel.TotalRooms), nameof(HotelFeaturedDealResponse.TotalRooms))]
    [MapProperty(nameof(RoomInfo.PricePerNight), nameof(HotelFeaturedDealResponse.OriginalPrice))]
    [MapProperty(nameof(RoomInfo.Hotel.HotelType), nameof(HotelFeaturedDealResponse.HotelType))]
    public partial HotelFeaturedDealResponse MapRoomInfoToHotelFeaturedDeal(RoomInfo roomInfo);
}