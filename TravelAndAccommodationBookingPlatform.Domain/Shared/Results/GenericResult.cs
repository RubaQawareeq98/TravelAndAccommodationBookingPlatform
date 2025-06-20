using TravelAndAccommodationBookingPlatform.Domain.Shared.Errors;

namespace TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

public class Result<T> : Result
{
    public T Value { get; }

    private Result(bool isSuccess, T value, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, Error.None);
    public new static Result<T> Failure(Error error) => new Result<T>(false, default!, error);
}
