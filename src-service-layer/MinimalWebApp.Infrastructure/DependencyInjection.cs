using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Infrastructure.Persistence;
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

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        return services;
    }
}
