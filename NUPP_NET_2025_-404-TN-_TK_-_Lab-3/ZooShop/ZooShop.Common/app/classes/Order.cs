using System;
using System.Collections.Generic;

namespace ZooShop.Common
{
    // делегат
    public delegate void OrderCreatedHandler(Order order);

    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Product> Productes { get; set; } = new List<Product>();
        public double TotalPrice { get; set; }

        // подія
        public event OrderCreatedHandler? OnOrderCreated;

        // конструктор
        public Order(List<Product> productes)
        {
            Id = Guid.NewGuid();
            OrderDate = DateTime.Now;
            Productes = productes;
            TotalPrice = CalculateTotal();

            // викликаємо подію після створення
            OnOrderCreated?.Invoke(this);
        }

        // метод
        private double CalculateTotal()
        {
            double total = 0;
            foreach (var product in Productes)
            {
                total += product.Price;
            }
            return total;
        }

        public string GetSummary()
        {
            return $"Замовлення {Id} від {OrderDate}, сума: {TotalPrice} грн";
        }
    }

    // Додаткові статичні методи
    public static partial class OrderFactory
    {
        public static Order GenerateSample(int productesCount = 2)
        {
            var rnd = Random.Shared;
            var productes = new List<Product>();
            for (int i = 0; i < productesCount; i++)
            {
                productes.Add(Product.GenerateSample());
            }
            return new Order(productes);
        }
    }
}
