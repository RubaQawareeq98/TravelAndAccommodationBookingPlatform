using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Discounts.Mappers;

[Mapper]
public partial class DiscountResponseMapper
{
    public partial DiscountResponse MapDiscountToDiscountResponse(Discount discount);
    public partial List<DiscountResponse>  MapDiscountListToDiscountResponseList(List<Discount> discounts);
}
