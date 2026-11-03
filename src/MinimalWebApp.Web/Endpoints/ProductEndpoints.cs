using MinimalWebApp.Application.Abstractions.Messaging;
using MinimalWebApp.Application.Products.Commands.CreateProduct;
using MinimalWebApp.Application.Products.Commands.DeleteProduct;
using MinimalWebApp.Application.Products.Commands.UpdateProduct;
using MinimalWebApp.Application.Products.Queries.GetProduct;
using MinimalWebApp.Application.Products.Queries.ListProducts;
using MinimalWebApp.Web.Extensions;

namespace MinimalWebApp.Web.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var products = app.MapGroup("/api/products").WithTags("Products");

        products.MapGet("/", async (ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new ListProductsQuery(), ct)));

        products.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            (await sender.Send(new GetProductQuery(id), ct)).Match(Results.Ok));

        // The command's shape matches the request body, so model binding builds it directly.
        products.MapPost("/", async (CreateProductCommand command, ISender sender, CancellationToken ct) =>
            (await sender.Send(command, ct))
                .Match(product => Results.Created($"/api/products/{product.Id}", product)));

        // The route owns the id: whatever the body claims is overwritten, so the two
        // can never disagree.
        products.MapPut("/{id:guid}", async (Guid id, UpdateProductCommand command, ISender sender, CancellationToken ct) =>
            (await sender.Send(command with { Id = id }, ct)).Match(Results.Ok));

        products.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            (await sender.Send(new DeleteProductCommand(id), ct)).Match(Results.NoContent));

        return app;
    }
}
