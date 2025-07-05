using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TAABP.integrationTests.Handlers;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;
using Testcontainers.MsSql;
using TravelAndAccommodationBookingPlatform.Api;

namespace TAABP.integrationTests.Fixtures;

public class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer;
    private IDbContextTransaction _transaction;

    public WebApplicationFactory<Program> Factory { get; private set; }

    public HttpClient Client { get; private set; }
    private string ConnectionString { get; set; } = string.Empty;

    public SqlServerFixture()
    {
        IConfiguration configuration =
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("testAppsettings.json", optional: false)
            .Build();

        var image = configuration["SqlServerContainer:Image"];
        var password = configuration["SqlServerContainer:Password"];

        _dbContainer = new MsSqlBuilder()
            .WithImage(image)
            .WithPassword(password)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        ConnectionString = _dbContainer.GetConnectionString();

        Factory = CreateFactoryWithOverrides();

        Client = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HotelBookingManagementDbContext>();
        await db.Database.EnsureCreatedAsync();
        _transaction = await db.Database.BeginTransactionAsync();
    }

    public async Task DisposeAsync()
    {
        try
        {
            await _transaction.RollbackAsync();
        }
        finally
        {
            Client.Dispose();
            await Factory.DisposeAsync();
            await _dbContainer.DisposeAsync();
        }
    }
    
    public WebApplicationFactory<Program> CreateFactoryWithOverrides(Action<IServiceCollection>? configureOverrides = null)
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<HotelBookingManagementDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);
                
                services.AddAuthentication("AuthScheme")
                    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandlerTest>(
                        "AuthScheme", _ => { });

                services.AddDbContext<HotelBookingManagementDbContext>(options =>
                    options.UseSqlServer(ConnectionString));

                // Allow caller to inject or override services
                configureOverrides?.Invoke(services);
            });
        });
    }
}
