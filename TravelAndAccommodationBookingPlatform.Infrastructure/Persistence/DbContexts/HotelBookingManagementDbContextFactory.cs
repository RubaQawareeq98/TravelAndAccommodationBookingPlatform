using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

public class HotelBookingManagementDbContextFactory : IDesignTimeDbContextFactory<HotelBookingManagementDbContext>
{
    public HotelBookingManagementDbContext CreateDbContext(string[] args)
    {
    var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../TravelAndAccommodationBookingPlatform.Api");

        var configuration = new ConfigurationBuilder()
          .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<HotelBookingManagementDbContext>();
        var connectionString = configuration.GetConnectionString("sqlConnectionString");

        optionsBuilder.UseSqlServer(connectionString);

        return new HotelBookingManagementDbContext(optionsBuilder.Options);
    }
}