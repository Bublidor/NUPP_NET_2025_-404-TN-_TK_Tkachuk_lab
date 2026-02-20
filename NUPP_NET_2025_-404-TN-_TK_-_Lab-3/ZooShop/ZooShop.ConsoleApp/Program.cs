using System;
using System.Linq;
using System.Threading.Tasks;
using ZooShop.Infrastructure;
using ZooShop.Infrastructure.Models;
using ZooShop.Infrastructure.Repositories;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("ZooShop Management System");
        Console.WriteLine("Repository Pattern + PostgreSQL\n");

        try
        {
            using var context = new ZooShopContext();
            using var unitOfWork = new UnitOfWork(context);

            await DemoCustomersAsync(unitOfWork);
            await DemoProductsAsync(unitOfWork);
            await DemoOrdersAsync(unitOfWork);
            await ShowStatisticsAsync(unitOfWork);

            Console.WriteLine("\n✅ Демонстрація завершена успішно!");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n❌ Помилка: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"Деталі: {ex.InnerException.Message}");
            Console.ResetColor();
        }
    }

    // -------------------- CUSTOMERS --------------------

    static async Task DemoCustomersAsync(IUnitOfWork unitOfWork)
    {
        Console.WriteLine("1️⃣ Робота з покупцями");

        var customers = (await unitOfWork.Customers.GetAllAsync()).ToList();

        Console.WriteLine($"Покупців у базі: {customers.Count}");

        foreach (var customer in customers)
        {
            Console.WriteLine($" - {customer.FullName}");
            Console.WriteLine($"   Телефон: {customer.PhoneNumber}");
            Console.WriteLine($"   Бонусні бали: {customer.LoyaltyPoints}");
        }

        var topCustomers = await unitOfWork.Customers.GetTopLoyalCustomersAsync(3);

        Console.WriteLine("\n🏆 Топ-3 покупці:");
        foreach (var customer in topCustomers)
        {
            Console.WriteLine($" {customer.FullName} — {customer.LoyaltyPoints} балів");
        }

        Console.WriteLine();
    }

    // -------------------- PRODUCTS --------------------

    static async Task DemoProductsAsync(IUnitOfWork unitOfWork)
    {
        Console.WriteLine("2️⃣ Робота з товарами");

        var products = (await unitOfWork.Products.GetAvailableProductsAsync()).ToList();

        Console.WriteLine($"Товарів у продажі: {products.Count}");

        foreach (var product in products)
        {
            var type = product.GetType().Name.Replace("Model", "");
            Console.WriteLine($" [{type}] {product.Name} — {product.Price:F2} грн");
        }

        var petFoods = await unitOfWork.Products.GetAllPetFoodsAsync();

        Console.WriteLine($"\n🐾 Корм для тварин ({petFoods.Count()}):");
        foreach (var food in petFoods)
        {
            Console.WriteLine($" {food.Name}");
            Console.WriteLine($"   Тип тварини: {food.AnimalType}");
            Console.WriteLine($"   Вага: {food.WeightKg} кг");
            Console.WriteLine($"   Ціна: {food.Price:F2} грн");
        }

        var accessories = await unitOfWork.Products.GetAllAccessoriesAsync();

        Console.WriteLine($"\n🎾 Аксесуари ({accessories.Count()}):");
        foreach (var item in accessories)
        {
            Console.WriteLine($" {item.Name}");
            Console.WriteLine($"   Матеріал: {item.Material}");
            Console.WriteLine($"   Вага: {item.Weight} кг");
            Console.WriteLine($"   Ціна: {item.Price:F2} грн");
        }

        Console.WriteLine();
    }

    // -------------------- ORDERS --------------------

    static async Task DemoOrdersAsync(IUnitOfWork unitOfWork)
    {
        Console.WriteLine("3️⃣ Робота із замовленнями");

        var orders = (await unitOfWork.Orders.GetRecentOrdersAsync(5)).ToList();

        foreach (var order in orders)
        {
            Console.WriteLine($"\nЗамовлення #{order.Id}");
            Console.WriteLine($" Дата: {order.OrderDate:dd.MM.yyyy HH:mm}");
            Console.WriteLine($" Покупець: {order.Customer.FullName}");
            Console.WriteLine($" Статус: {order.Status}");
            Console.WriteLine($" Сума: {order.TotalPrice:F2} грн");
        }

        var pendingOrders = await unitOfWork.Orders.GetOrdersByStatusAsync("Pending");
        Console.WriteLine($"\nЗамовлень в обробці: {pendingOrders.Count()}");

        Console.WriteLine();
    }

    // -------------------- STATISTICS --------------------

    static async Task ShowStatisticsAsync(IUnitOfWork unitOfWork)
    {
        Console.WriteLine("4️⃣ Статистика");

        var products = (await unitOfWork.Products.GetAllAsync()).ToList();

        if (products.Any())
        {
            Console.WriteLine("\n📦 Статистика по товарах:");
            Console.WriteLine($" Всього товарів: {products.Count}");
            Console.WriteLine($" Мін. ціна: {products.Min(p => p.Price):F2} грн");
            Console.WriteLine($" Макс. ціна: {products.Max(p => p.Price):F2} грн");
            Console.WriteLine($" Середня ціна: {products.Average(p => p.Price):F2} грн");
        }

        var orders = (await unitOfWork.Orders.GetAllAsync()).ToList();

        if (orders.Any())
        {
            Console.WriteLine("\n📊 Статистика по замовленнях:");
            Console.WriteLine($" Всього замовлень: {orders.Count}");
            Console.WriteLine($" Загальний дохід: {orders.Sum(o => o.TotalPrice):F2} грн");
            Console.WriteLine($" Середній чек: {orders.Average(o => o.TotalPrice):F2} грн");
        }

        var customers = (await unitOfWork.Customers.GetAllAsync()).ToList();

        if (customers.Any())
        {
            Console.WriteLine("\n👥 Статистика по покупцях:");
            Console.WriteLine($" Всього покупців: {customers.Count}");
            Console.WriteLine($" Середні бонусні бали: {customers.Average(c => c.LoyaltyPoints):F2}");
        }
    }
}

