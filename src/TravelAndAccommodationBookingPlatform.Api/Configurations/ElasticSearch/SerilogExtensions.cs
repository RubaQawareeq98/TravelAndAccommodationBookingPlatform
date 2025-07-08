using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace TravelAndAccommodationBookingPlatform.Api.Configurations.ElasticSearch;

public static class SerilogExtensions
{
    public static void AddSerilogWithElasticSearch(this IHostBuilder hostBuilder,
        ElasticSearchConfigurations configurations)
    {
        Console.WriteLine("Using Elastic Search Configurations");
            Console.WriteLine(configurations.ElasticSearchUri);
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configurations.ElasticSearchUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "myapp-logs-{0:yyyy.MM.dd}",
                ModifyConnectionSettings = x => x.BasicAuthentication(configurations.Username, configurations.Password)
            })
            .CreateLogger();

        hostBuilder.UseSerilog();
    }
}
