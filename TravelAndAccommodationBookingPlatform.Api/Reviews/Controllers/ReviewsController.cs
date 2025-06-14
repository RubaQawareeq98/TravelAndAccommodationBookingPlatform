using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Reviews.Controllers;

[Route("api/reviews")]
[ApiController]
public class ReviewsController(IReviewService reviewService, ReviewRequestMapper reviewRequestMapper) : ControllerBase
{
    /// <summary>
    /// Return list of reviews with pagination, filtering, sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <returns>list of available reviews</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Review>>> GetReviews([FromQuery] SieveModel sieveModel)
    {
        var reviews = await reviewService.GetReviews(sieveModel);
        return Ok(reviews);
    }

    /// <summary>
    /// Return review by review id if review id exist
    /// </summary>
    /// <param name="reviewId"></param>
    ///  /// <response code="200">If the review exist.</response>
    /// <response code="404">If the review not exist.</response>
    /// <returns>review if exist or not found</returns>
    [HttpGet("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Review>> GetReview([FromRoute] Guid reviewId)
    {
        var review = await reviewService.GetReviewById(reviewId);
        return Ok(review);
    }

    /// <summary>
    /// Add new review with valid data
    /// </summary>
    /// <param name="addReviewRequest"></param>
    /// <response code="201">If the review created.</response>
    /// <response code="400">If the review data not valid.</response>
    /// <returns>created review</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddReview([FromBody] AddReviewRequest addReviewRequest)
    {
        var review = reviewRequestMapper.MapAddReviewRequestToReview(addReviewRequest);
        await reviewService.AddReview(review);
        
        return CreatedAtAction(nameof(GetReview),
            new { reviewId = review.Id }, review);
    }
    
    /// <summary>
    /// Partially update the review information
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="reviewPatchDocument"></param>
    /// <response code="204">If review updated successfully.</response>
    /// <response code="404">If the review not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReview([FromRoute] Guid reviewId, JsonPatchDocument<UpdateReviewRequest> reviewPatchDocument)
    {
        var review = await reviewService.GetReviewById(reviewId);

        var updateReviewRequest = reviewRequestMapper.MapReviewToUpdateReviewRequest(review);
        reviewPatchDocument.ApplyTo(updateReviewRequest);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        reviewRequestMapper.MapUpdateReviewRequestToReview(updateReviewRequest, review);
        
        await reviewService.UpdateReview(review);
        return NoContent();
    }

    /// <summary>
    /// Soft delete review by review id
    /// </summary>
    /// <param name="reviewId"></param>
    /// <response code="204">If the review deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if review deleted successfully or not found.</returns>
    [HttpDelete("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReview([FromRoute] Guid reviewId)
    {
        await reviewService.DeleteReview(reviewId);
        return NoContent();
    }
}
