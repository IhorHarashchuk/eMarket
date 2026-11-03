using Microsoft.Extensions.DependencyInjection;
using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Application.Messaging;

namespace MinimalWebApp.Application;

public static class DependencyInjection
{
    /// <summary>Registers the dispatcher and every handler in this assembly. No infrastructure is wired up here.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<ISender, Sender>();

        services.AddHandlers(typeof(ICommandHandler<,>));
        services.AddHandlers(typeof(IQueryHandler<,>));

        return services;
    }

    /// <summary>
    /// Registers each handler under the closed interface it implements, which is the
    /// exact service type <see cref="Sender"/> asks for. Scanning means a new use case
    /// is wired up by existing, not by editing this file.
    /// </summary>
    private static void AddHandlers(this IServiceCollection services, Type openHandlerType)
    {
        var candidates = typeof(DependencyInjection).Assembly
            .GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false, IsGenericTypeDefinition: false });

        foreach (var candidate in candidates)
        {
            var implemented = candidate.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openHandlerType);

            foreach (var handlerInterface in implemented)
            {
                services.AddScoped(handlerInterface, candidate);
            }
        }
    }
}
