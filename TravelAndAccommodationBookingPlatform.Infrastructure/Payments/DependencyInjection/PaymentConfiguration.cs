using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Payments;
using TravelAndAccommodationBookingPlatform.Infrastructure.Payments.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Payments.DependencyInjection;

public static class PaymentConfiguration
{
    public static IServiceCollection AddPaymentService(this IServiceCollection services)
    {
        services.AddScoped<IPaymentService, StripePaymentService>();
        
        return services;
    }
}
