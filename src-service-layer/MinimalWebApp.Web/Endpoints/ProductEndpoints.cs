using MinimalWebApp.Application.Products;
using MinimalWebApp.Web.Extensions;

namespace MinimalWebApp.Web.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var products = app.MapGroup("/api/products").WithTags("Products");

        products.MapGet("/", async (IProductService service, CancellationToken ct) =>
            Results.Ok(await service.ListAsync(ct)));

        products.MapGet("/{id:guid}", async (Guid id, IProductService service, CancellationToken ct) =>
            (await service.GetAsync(id, ct)).Match(Results.Ok));

        products.MapPost("/", async (ProductRequest request, IProductService service, CancellationToken ct) =>
            (await service.CreateAsync(request, ct))
                .Match(product => Results.Created($"/api/products/{product.Id}", product)));

        products.MapPut("/{id:guid}", async (Guid id, ProductRequest request, IProductService service, CancellationToken ct) =>
            (await service.UpdateAsync(id, request, ct)).Match(Results.Ok));

        products.MapDelete("/{id:guid}", async (Guid id, IProductService service, CancellationToken ct) =>
            (await service.DeleteAsync(id, ct)).Match(Results.NoContent));

        return app;
    }
}
