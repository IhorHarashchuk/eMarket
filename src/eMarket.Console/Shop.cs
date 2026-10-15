using System.Runtime.InteropServices;
using eMarket.Console.Models;

namespace eMarket.Console;

public static class Shop
{
    public static Product CreateProduct(int id, string name, decimal price, int stock, string category)
    {
     if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.", nameof(name));

        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be positive.");

        if (stock < 0)
            throw new ArgumentOutOfRangeException(nameof(stock), "Stock cannot be negative.");

        return new Product
        {
            Id = id,
            Name = name.Trim(),
            Price = price,
            StockQuantity = stock,
            Category = category
        };   
    }

     public static void PrintProduct(Product product)
    {
        var availability = CheckStock(product, 1) ? "in stock" : "out of stock";
        System.Console.WriteLine($"#{product.Id,-3} {product.Name,-24} {product.Price,10:C} {availability}");
    }

    // Скидка по сумме заказа: 0% / 5% / 10%
    public static decimal CalculateDiscount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        var percent = amount switch
        {
            >= 1000 => 0.10m,
            >= 500 => 0.05m,
            _ => 0m
        };

        return decimal.Round(amount * percent, 2);
    }

    public static bool CheckStock(Product product, int quantity) =>
        quantity > 0 && product.StockQuantity >= quantity;

    public static decimal GetTotal(decimal amount) => amount - CalculateDiscount(amount);
}