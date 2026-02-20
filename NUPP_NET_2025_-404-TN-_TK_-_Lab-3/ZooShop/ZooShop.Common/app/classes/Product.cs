using System;

namespace ZooShop.Common
{
    public class Product : IHasId
    {
        // статичне поле
        public static int TotalProductesCreated;

        // властивості
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        // конструктор
        public Product(string name, double price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            TotalProductesCreated++;
        }

        // статичний конструктор
        static Product()
        {
            TotalProductesCreated = 0;
        }

        // метод
        public virtual string GetInfo()
        {
            return $"{Name} - {Price} грн";
        }

        // статичний метод
        public static int GetTotalProductesCount()
        {
            return TotalProductesCreated;
        }

        // Генерує приклад Product
        public static Product GenerateSample()
        {
            var rnd = Random.Shared;
            var price = Math.Round(rnd.NextDouble() * 400 + 50, 2); // 50..450
            return new Product($"Product {rnd.Next(100, 999)}", price);
        }

        // CreateNew wrapper
        public static Product CreateNew() => GenerateSample();
    }
}
