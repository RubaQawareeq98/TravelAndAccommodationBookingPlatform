using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Emails;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Images;
using TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Services;
using TravelAndAccommodationBookingPlatform.Infrastructure.Images;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailMessageGeneratorService, EmailMessageGeneratorService>();
        
        return services;
    }
}