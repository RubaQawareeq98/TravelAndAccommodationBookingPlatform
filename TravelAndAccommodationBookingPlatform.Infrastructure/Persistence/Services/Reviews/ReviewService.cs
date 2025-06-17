using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Reviews;

public class ReviewService(IReviewRepository reviewRepository, IHotelService hotelService) : IReviewService
{
    public async Task AddReview(Guid hotelId, Review review)
    {
        var hotel = await hotelService.GetHotelById(hotelId);
        
        review.HotelId = hotel.Id;
        await reviewRepository.AddReview(review);
    }

    public async Task UpdateReview(Review review)
    {
        await reviewRepository.UpdateReview(review);
    }

    public async Task DeleteReview(Guid hotelId, Guid reviewId)
    {
        var review = await GetReviewById(hotelId, reviewId);
        await reviewRepository.DeleteReview(review);
    }
    
    public async Task<Review> GetReviewById(Guid hotelId, Guid reviewId)
    {
        var review = await reviewRepository.GetReview(hotelId, reviewId);
        if (review is null)
        {
            throw new NotFoundException($"Review with if {reviewId} not found");
        }
        
        return review;
    }

    public async Task<List<Review>> GetReviews(SieveModel sieveModel, Guid hotelId)
    {
        var hotel = await hotelService.GetHotelById(hotelId);
        
        return await reviewRepository.GetAllReviews(sieveModel, hotel.Id);
    }
}
