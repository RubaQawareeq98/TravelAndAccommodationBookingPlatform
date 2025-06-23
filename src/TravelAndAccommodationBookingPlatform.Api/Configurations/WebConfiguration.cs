namespace TravelAndAccommodationBookingPlatform.Api.Configurations;

public static class WebConfiguration
{
    public static IServiceCollection AddWebConfigurations(this IServiceCollection services)
    {
        return services
            .AddValidatorsConfigurations()
            .AddMapperConfigurations();
    }
}
