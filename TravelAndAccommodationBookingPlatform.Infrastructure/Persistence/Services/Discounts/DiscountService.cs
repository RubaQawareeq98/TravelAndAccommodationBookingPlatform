using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Discounts;

public class DiscountService(IDiscountRepository discountRepository) : IDiscountService
{
    public async Task AddDiscount(Discount discount)
    {
        await discountRepository.AddDiscount(discount);
    }

    public async Task UpdateDiscount(Discount discount)
    {
        await discountRepository.UpdateDiscount(discount);
    }

    public async Task DeleteDiscount(Guid discountId)
    {
        var discount = await GetDiscountById(discountId);
        await discountRepository.DeleteDiscount(discount);
    }

    public async Task<Discount> GetDiscountById(Guid discountId)
    {
        var discount = await discountRepository.GetDiscount(discountId);
        if (discount is null)
        {
            throw new NotFoundException($"Discount with if {discountId} not found");
        }
        
        return discount;
    }

    public async Task<List<Discount>> GetDiscounts(SieveModel sieveModel)
    {
        return await discountRepository.GetAllDiscounts(sieveModel);
    }
}
