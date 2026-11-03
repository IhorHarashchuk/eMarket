using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Infrastructure.Persistence;
using MinimalWebApp.Infrastructure.Persistence.Queries;
using MinimalWebApp.Infrastructure.Persistence.Repositories;

namespace MinimalWebApp.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Binds the application's persistence abstractions to EF Core + PostgreSQL.</summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseSnakeCaseNamingConvention());

        // Write side.
        services.AddScoped<IProductRepository, ProductRepository>();
        // Resolve to the same AppDbContext instance the repository uses, otherwise the
        // tracked changes and the save would happen on two different contexts.
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        // Read side. Shares the DbContext today; could point at a replica or Dapper tomorrow
        // without the application layer noticing.
        services.AddScoped<IProductReader, ProductReader>();

        return services;
    }
}
