using Microsoft.EntityFrameworkCore;
using MinimalWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseSnakeCaseNamingConvention());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Serve the browser UI from wwwroot (index.html at "/").
app.UseDefaultFiles();
app.UseStaticFiles();

var products = app.MapGroup("/api/products");

// READ all
products.MapGet("/", async (AppDbContext db) =>
    await db.Products.OrderBy(p => p.Sku).ToListAsync());

// READ one
products.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
    await db.Products.FindAsync(id) is { } product
        ? Results.Ok(product)
        : Results.NotFound());

// CREATE
products.MapPost("/", async (ProductInput input, AppDbContext db) =>
{
    if (Validate(input) is { } error)
    {
        return error;
    }

    var sku = input.Sku.Trim().ToUpperInvariant();

    if (await db.Products.AnyAsync(p => p.Sku == sku))
    {
        return Results.Conflict($"A product with SKU '{sku}' already exists.");
    }

    var product = new Product
    {
        Id = Guid.CreateVersion7(),
        Sku = sku,
        Name = input.Name.Trim(),
        Description = Normalize(input.Description),
        Price = decimal.Round(input.Price, 2),
        CreatedAtUtc = DateTimeOffset.UtcNow
    };

    db.Products.Add(product);
    await db.SaveChangesAsync();

    return Results.Created($"/api/products/{product.Id}", product);
});

// UPDATE
products.MapPut("/{id:guid}", async (Guid id, ProductInput input, AppDbContext db) =>
{
    if (Validate(input) is { } error)
    {
        return error;
    }

    var product = await db.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound();
    }

    var sku = input.Sku.Trim().ToUpperInvariant();

    if (await db.Products.AnyAsync(p => p.Sku == sku && p.Id != id))
    {
        return Results.Conflict($"A product with SKU '{sku}' already exists.");
    }

    product.Sku = sku;
    product.Name = input.Name.Trim();
    product.Description = Normalize(input.Description);
    product.Price = decimal.Round(input.Price, 2);
    product.UpdatedAtUtc = DateTimeOffset.UtcNow;

    await db.SaveChangesAsync();

    return Results.Ok(product);
});

// DELETE
products.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null)
    {
        return Results.NotFound();
    }

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();

static IResult? Validate(ProductInput input)
{
    var errors = new Dictionary<string, string[]>();

    if (string.IsNullOrWhiteSpace(input.Sku))
    {
        errors["sku"] = ["SKU is required."];
    }
    else if (input.Sku.Trim().Length > 32)
    {
        errors["sku"] = ["SKU must be 32 characters or fewer."];
    }

    if (string.IsNullOrWhiteSpace(input.Name))
    {
        errors["name"] = ["Name is required."];
    }
    else if (input.Name.Trim().Length > 200)
    {
        errors["name"] = ["Name must be 200 characters or fewer."];
    }

    if (input.Description?.Trim().Length > 2000)
    {
        errors["description"] = ["Description must be 2000 characters or fewer."];
    }

    if (input.Price < 0m)
    {
        errors["price"] = ["Price cannot be negative."];
    }

    return errors.Count > 0 ? Results.ValidationProblem(errors) : null;
}

static string? Normalize(string? value) =>
    string.IsNullOrWhiteSpace(value) ? null : value.Trim();

record ProductInput(string Sku, string Name, string? Description, decimal Price);
