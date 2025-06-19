using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Images;
using TravelAndAccommodationBookingPlatform.Infrastructure.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IImageService, ImageService>();
        
        return services;
    }
}