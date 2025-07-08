using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Discounts;

public class DiscountService(IDiscountRepository discountRepository,
    IRoomCategoryService roomCategoryService) : IDiscountService
{
    public async Task<Result<Discount>> AddDiscount(Guid hotelId, Guid roomCategoryId, Discount discount,
        CancellationToken cancellationToken)
    {
        var roomCategoryResult = await roomCategoryService.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken);
        if (roomCategoryResult.IsFailure)
        {
            return Result<Discount>.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId));
        }

        discount.RoomCategoryId = roomCategoryId;
        await discountRepository.AddDiscount(discount, cancellationToken);
        return Result<Discount>.Success(discount);
    }

    public async Task<Result> DeleteDiscount(Guid hotelId, Guid roomCategoryId, Guid discountId, CancellationToken cancellationToken)
    {
        var roomCategoryResult = await roomCategoryService.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken);
        if (roomCategoryResult.IsFailure)
        {
            return Result.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId));
        }
        
        var discount = await discountRepository.GetDiscount(roomCategoryId, discountId, cancellationToken);
        if (discount is null)
        {
            return Result.Failure(DiscountError.DiscountNotFound(discountId));
        }
        
        await discountRepository.DeleteDiscount(discount, cancellationToken);
        return Result.Success();
    }

    public async Task UpdateDiscount(Discount discount)
    {
        await discountRepository.UpdateDiscount(discount);
    }
    
    public async Task<Result<List<Discount>>> GetDiscountsByRoom(Guid hotelId, Guid roomCategoryId,
        SieveModel sieveModel, CancellationToken cancellationToken = default)
    {
        var roomCategoryResult = await roomCategoryService.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken);
        if (roomCategoryResult.IsFailure)
        {
            return Result<List<Discount>>.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId));
        }
        
        var discounts = await discountRepository.GetAllDiscountsByRoom(roomCategoryId, sieveModel, cancellationToken);
        return Result<List<Discount>>.Success(discounts);
    }

    public async Task<Result<Discount>> GetDiscountById(Guid hotelId, Guid roomCategoryId, Guid discountId,
        CancellationToken cancellationToken)
    {
        var roomCategoryResult = await roomCategoryService.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken);
        if (roomCategoryResult.IsFailure)
        {
            return Result<Discount>.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId));
        }
        
        var discount = await discountRepository.GetDiscount(roomCategoryId, discountId, cancellationToken);
        return discount is null ? Result<Discount>.Failure(DiscountError.DiscountNotFound(discountId)) : Result<Discount>.Success(discount);
    }
}
