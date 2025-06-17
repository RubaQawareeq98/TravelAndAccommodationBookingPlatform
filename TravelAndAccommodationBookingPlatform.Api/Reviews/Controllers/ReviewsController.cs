using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Reviews.Controllers;

/// <summary>
/// Controllers for hotel reviews endpoints
/// </summary>
/// <param name="reviewService"></param>
/// <param name="reviewRequestMapper"></param>
/// <param name="reviewResponseMapper"></param>
[Route("api/hotels/{hotelId:guid}/reviews")]
[ApiController]
public class ReviewsController(IReviewService reviewService,
    ReviewRequestMapper reviewRequestMapper,
    ReviewResponseMapper reviewResponseMapper) : ControllerBase
{
    /// <summary>
    /// Return list of reviews with pagination, filtering, sorting
    /// </summary>
    /// <param name="sieveModel"></param>
    /// <param name="hotelId"></param>
    /// <returns>list of available reviews</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Review>>> GetReviews([FromQuery] SieveModel sieveModel, [FromRoute] Guid hotelId)
    {
        var reviews = await reviewService.GetReviews(sieveModel, hotelId);
        var reviewsResponse = reviewResponseMapper.MapReviewListToReviewResponseList(reviews);
        return Ok(reviewsResponse);
    }

    /// <summary>
    /// Return review by review id if review id exist
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="hotelId"></param>
    /// /// <response code="200">If the review exist.</response>
    /// <response code="404">If the review not exist.</response>
    /// <returns>review if exist or not found</returns>
    [HttpGet("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Review>> GetReview([FromRoute] Guid reviewId, [FromRoute] Guid hotelId)
    {
        var review = await reviewService.GetReviewById(hotelId, reviewId);
        var reviewResponse = reviewResponseMapper.MapReviewToReviewResponse(review);
        return Ok(reviewResponse);
    }

    /// <summary>
    /// Add new review with valid data
    /// </summary>
    /// <param name="addReviewRequest"></param>
    /// <param name="hotelId"></param>
    /// <response code="201">If the review created.</response>
    /// <response code="400">If the review data not valid.</response>
    /// <returns>created review</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddReview([FromBody] AddReviewRequest addReviewRequest, Guid hotelId)
    {
        var review = reviewRequestMapper.MapAddReviewRequestToReview(addReviewRequest);
        await reviewService.AddReview(hotelId, review);
        
        var reviewResponse = reviewResponseMapper.MapReviewToReviewResponse(review);
        return CreatedAtAction(nameof(GetReview),
            new { reviewId = review.Id, hotelId }, reviewResponse);
    }

    /// <summary>
    /// Partially update the review information
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="reviewPatchDocument"></param>
    /// <param name="hotelId"></param>
    /// <response code="204">If review updated successfully.</response>
    /// <response code="404">If the review not exist.</response>
    /// <returns>No content if updated successfully or not found.</returns>
    [HttpPatch("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReview([FromRoute] Guid reviewId, JsonPatchDocument<UpdateReviewRequest> reviewPatchDocument, Guid hotelId)
    {
        var review = await reviewService.GetReviewById(hotelId, reviewId);

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
    /// Delete review by review id
    /// </summary>
    /// <param name="reviewId"></param>
    /// <param name="hotelId"></param>
    /// <response code="204">If the review deleted successfully.</response>
    /// <response code="404">If the hotel not exist.</response>
    /// <returns>No content if review deleted successfully or not found.</returns>
    [HttpDelete("{reviewId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReview([FromRoute] Guid reviewId, Guid hotelId)
    {
        await reviewService.DeleteReview(hotelId, reviewId);
        return NoContent();
    }
}
