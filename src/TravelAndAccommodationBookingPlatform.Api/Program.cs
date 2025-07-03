using CloudinaryDotNet;
using TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using QuestPDF.Infrastructure;
using Serilog;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Api.Configurations;
using TravelAndAccommodationBookingPlatform.Api.Configurations.ElasticSearch;
using TravelAndAccommodationBookingPlatform.Api.Middlewares;
using TravelAndAccommodationBookingPlatform.Application.Configurations;

var builder = WebApplication.CreateBuilder(args);


var elasticSearchConfig = builder.Configuration
    .GetSection("ElasticSearch")
    .Get<ElasticSearchConfigurations>();

if (elasticSearchConfig is not null)
{;
    Console.WriteLine("kkkkk");
    builder.Host.AddSerilogWithElasticSearch(elasticSearchConfig);
}
else
{
    Console.WriteLine("noooo");

    builder.Host.UseSerilog((_, lc) => lc
        .WriteTo.Console()
        .Enrich.FromLogContext());
}

QuestPDF.Settings.License = LicenseType.Community;

builder.AddInfrastructureConfigurations(builder.Configuration)
    .AddApplication();
builder.AddWebConfigurations();

builder.Services.AddControllers().AddNewtonsoftJson();



builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();



QuestPDF.Settings.License = LicenseType.Community;


builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    return new Account(config.CloudName, config.ApiKey, config.ApiSecret);
});

builder.Services.AddSwaggerGen();

// var serviceProvider = builder.Services.BuildServiceProvider();
// await BookingService.TestConcurrentBookings(serviceProvider);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

await app.RunAsync();