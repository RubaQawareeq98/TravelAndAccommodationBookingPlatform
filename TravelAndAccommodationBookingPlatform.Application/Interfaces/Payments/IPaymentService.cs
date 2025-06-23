using TravelAndAccommodationBookingPlatform.Application.Interfaces.Payments.Dtos;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Payments;

public interface IPaymentService
{
    Task<Result<string>> CreatePaymentService(AddPaymentRequest request);
}
