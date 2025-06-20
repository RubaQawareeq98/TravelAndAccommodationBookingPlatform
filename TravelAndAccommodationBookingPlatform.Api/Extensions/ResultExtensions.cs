using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Api.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        if (result.Error.Type == ErrorType.NotFound)
            return new NotFoundObjectResult(new { error = result.Error.Message });
        
        return new BadRequestObjectResult(new { error = result.Error });
    }
    
    public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> onSuccess)
    {
        if (result.IsSuccess)
        {
            return onSuccess(result.Value);
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(new { error = result.Error.Message }),
            ErrorType.Conflict => new ConflictObjectResult(new { error = result.Error.Message }),
            _ => new BadRequestObjectResult(new { error = result.Error.Message })
        };
    }

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkResult();
        }
        if (result.Error.Type == ErrorType.NotFound)
            return new NotFoundObjectResult(new { error = result.Error.Message });

        return new BadRequestObjectResult(new { error = result.Error });
    }
    
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapping)
    {
        return result.IsSuccess
            ? Result<TOut>.Success(mapping(result.Value))
            : Result<TOut>.Failure(result.Error);
    }
}