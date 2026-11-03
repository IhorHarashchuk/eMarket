using Microsoft.Extensions.DependencyInjection;
using MinimalWebApp.Application.Products;

namespace MinimalWebApp.Application;

public static class DependencyInjection
{
    /// <summary>Registers the use-case services. No infrastructure is wired up here.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
