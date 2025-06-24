using CloudinaryDotNet;
using TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using QuestPDF.Infrastructure;
using Sieve.Services;
using TravelAndAccommodationBookingPlatform.Api.Configurations;
using TravelAndAccommodationBookingPlatform.Api.Middlewares;
using TravelAndAccommodationBookingPlatform.Application.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
    
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

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