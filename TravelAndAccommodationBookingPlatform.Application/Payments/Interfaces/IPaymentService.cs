using TravelAndAccommodationBookingPlatform.Application.Payments.Dtos;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Application.Payments.Interfaces;

public interface IPaymentService
{
    Task<Result<string>> CreatePaymentService(AddPaymentRequest request);
}
