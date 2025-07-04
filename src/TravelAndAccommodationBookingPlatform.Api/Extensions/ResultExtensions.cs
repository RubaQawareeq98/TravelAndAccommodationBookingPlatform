using TravelAndAccommodationBookingPlatform.Domain.Shared.Enums;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace TravelAndAccommodationBookingPlatform.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : CreateErrorResult(result.Error);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> onSuccess)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : CreateErrorResult(result.Error);
    }

    public static IActionResult ToActionResult(this Result result)
    {
        return result.IsSuccess
            ? new NoContentResult()
            : CreateErrorResult(result.Error);
    }

    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapping)
    {
        return result.IsSuccess
            ? Result<TOut>.Success(mapping(result.Value))
            : Result<TOut>.Failure(result.Error);
    }

    private static ObjectResult CreateErrorResult(Error error)
    {
        var payload = new { error = error.Message };

        return error.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(payload),
            ErrorType.Conflict => new ConflictObjectResult(payload),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(payload),
            ErrorType.Forbidden => new ObjectResult(payload) { StatusCode = StatusCodes.Status403Forbidden },
            ErrorType.BadRequest => new BadRequestObjectResult(payload),
            ErrorType.Internal => new ObjectResult(payload) { StatusCode = StatusCodes.Status500InternalServerError },
            _ => new BadRequestObjectResult(payload) 
        };
    }
}
