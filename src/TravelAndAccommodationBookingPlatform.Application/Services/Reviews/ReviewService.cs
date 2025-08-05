using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Application.Services.Reviews;

public class ReviewService(IReviewRepository reviewRepository,
    IHotelService hotelService,
    IUserService userService) : IReviewService
{
    public async Task<Result<Review>> AddReview(Guid hotelId, Review review, CancellationToken cancellationToken = default)
    {
        var hotelResult = await hotelService.GetHotelById(hotelId, cancellationToken);
        if (hotelResult.IsFailure)
        {
            return Result<Review>.Failure(HotelError.HotelNotFound(hotelId));
        }
        var hotel = hotelResult.Value;
        
        var userResult = await userService.GetUserById(review.UserId);
        if (userResult.IsFailure)
        {
            return Result<Review>.Failure(UserError.UserNotFoundById(review.UserId));
        }
        
        review.HotelId = hotel.Id;
        await reviewRepository.AddReview(review, cancellationToken);

        return Result<Review>.Success(review);
    }

    public async Task UpdateReview(Review review, CancellationToken cancellationToken = default)
    {
        await reviewRepository.UpdateReview(review, cancellationToken);
    }

    public async Task<Result> DeleteReview(Guid hotelId, Guid reviewId, CancellationToken cancellationToken = default)
    {
        var reviewResult = await GetReviewById(hotelId, reviewId, cancellationToken);
        if (reviewResult.IsFailure)
        {
            return Result.Failure(ReviewError.ReviewNotFound(reviewId));
        }
        
        var review = reviewResult.Value;
        await reviewRepository.DeleteReview(review, cancellationToken);
        return Result.Success();
    }
    
    public async Task<Result<Review>> GetReviewById(Guid hotelId, Guid reviewId, CancellationToken cancellationToken = default)
    {
        var review = await reviewRepository.GetReview(hotelId, reviewId, cancellationToken);
        return review is null ? Result<Review>.Failure(ReviewError.ReviewNotFound(reviewId)) : Result<Review>.Success(review);
    }

    public async Task<Result<List<Review>>> GetReviews(SieveModel sieveModel, Guid hotelId, CancellationToken cancellationToken = default)
    {
        var hotelResult = await hotelService.GetHotelById(hotelId, cancellationToken);
        if (hotelResult.IsFailure)
        {
            return Result<List<Review>>.Failure(HotelError.HotelNotFound(hotelId));
        }
        var hotel = hotelResult.Value;
        var reviews = await reviewRepository.GetAllReviews(sieveModel, hotel.Id, cancellationToken);
        
        return Result<List<Review>>.Success(reviews);
    }

    public async Task<Result<decimal>> CalculateHotelAverageRating(Guid hotelId, CancellationToken cancellationToken = default)
    {
        var hotelResult = await hotelService.GetHotelById(hotelId, cancellationToken);
        if (hotelResult.IsFailure)
        {
            return Result<decimal>.Failure(HotelError.HotelNotFound(hotelId));
        }

        var avgRating = await reviewRepository.GetAverageRating(hotelId, cancellationToken);
        return Result<decimal>.Success(avgRating);
    }
}
