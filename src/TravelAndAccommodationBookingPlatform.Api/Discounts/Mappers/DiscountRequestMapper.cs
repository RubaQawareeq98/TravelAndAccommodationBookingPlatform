using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Discounts.Mappers;

[Mapper]
public partial class DiscountRequestMapper
{
    public partial Discount MapAddDiscountRequestToDiscount(AddDiscountRequest addDiscountRequest);
    public partial void MapUpdateDiscountRequestToDiscount(UpdateDiscountRequest updateDiscountRequest, Discount discount);
    public partial UpdateDiscountRequest MapDiscountToUpdateDiscountRequest(Discount discount);
}
