using Microsoft.EntityFrameworkCore;
using MinimalWebApp.Application.Abstractions.Persistence;
using MinimalWebApp.Domain.Products;

namespace MinimalWebApp.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
