using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Emails.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Images.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.InvoiceDocuments.Interfaces;
using TravelAndAccommodationBookingPlatform.Infrastructure.Emails.Services;
using TravelAndAccommodationBookingPlatform.Infrastructure.Images;
using TravelAndAccommodationBookingPlatform.Infrastructure.InvoiceDocuments;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailMessageGeneratorService, EmailMessageGeneratorService>();
        services.AddScoped<IInvoiceGenerator, InvoiceGenerator>();
        
        return services;
    }
}