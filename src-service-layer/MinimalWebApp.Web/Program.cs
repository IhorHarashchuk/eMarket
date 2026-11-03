using MinimalWebApp.Application;
using MinimalWebApp.Infrastructure;
using MinimalWebApp.Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

// Composition root: the only place that knows about every layer.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

// Serve the browser UI from wwwroot (index.html at "/").
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapProductEndpoints();

app.Run();
