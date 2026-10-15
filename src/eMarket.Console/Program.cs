using eMarket.Console;
using eMarket.Console.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var products = new[]
{
    Shop.CreateProduct(1, "Mechanical keyboard", 129.90m, 12, "Peripherals"),
    Shop.CreateProduct(2, "27\" 4K monitor", 489.00m, 3, "Displays"),
    Shop.CreateProduct(3, "USB-C dock", 189.50m, 0, "Peripherals"),
    Shop.CreateProduct(4, "Noise cancelling headset", 249.00m, 7, "Audio")
};

var user = new User { Id = 1, Name = "Ihor", Email = "ihor@example.com" };
var order = new Order { Id = 1, UserId = user.Id };

var running = true;
while (running)
{
    PrintMenu();
    switch (Console.ReadLine()?.Trim())
    {
        case "1":
            foreach (var product in products)
                Shop.PrintProduct(product);
            break;

        case "2":
            Console.Write("product id: ");
            var id = int.TryParse(Console.ReadLine(), out var parsed) ? parsed : 0;
            var selected = Array.Find(products, p => p.Id == id);

            if (selected is null)
            {
                Console.WriteLine("Product not found.");
                break;
            }

            if (!Shop.CheckStock(selected, 1))
            {
                Console.WriteLine($"{selected.Name} is out of stock.");
                break;
            }

            selected.StockQuantity--;
            order.TotalPrice += selected.Price;
            Console.WriteLine($"Added {selected.Name}. Subtotal: {order.TotalPrice:C}");
            break;

        case "3":
            var discount = Shop.CalculateDiscount(order.TotalPrice);
            Console.WriteLine($"Order #{order.Id} for {user.Name} [{order.Status}]");
            Console.WriteLine($"  subtotal : {order.TotalPrice:C}");
            Console.WriteLine($"  discount : -{discount:C}");
            Console.WriteLine($"  total    : {Shop.GetTotal(order.TotalPrice):C}");
            break;

        case "0":
        case "exit":
            running = false;
            break;

        default:
            Console.WriteLine("Unknown command.");
            break;
    }
}

static void PrintMenu()
{
    Console.WriteLine();
    Console.WriteLine("=== eMarket ===");
    Console.WriteLine("1) Products");
    Console.WriteLine("2) Add to order");
    Console.WriteLine("3) Order summary");
    Console.WriteLine("0) Exit");
    Console.Write("> ");
}