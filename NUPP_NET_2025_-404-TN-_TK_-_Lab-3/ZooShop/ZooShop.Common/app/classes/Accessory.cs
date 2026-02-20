namespace ZooShop.Common
{
    public class Accessory : Product
    {
        public bool IsVegetarian { get; set; }
        public int Quantitys { get; set; }
        public string Dressing { get; set; }

        // конструктор (викликає базовий)
        public Accessory(string name, double price, bool isVegetarian, int quantitys, string dressing)
            : base(name, price)
        {
            IsVegetarian = isVegetarian;
            Quantitys = quantitys;
            Dressing = dressing;
        }

        // метод (override)
        public override string GetInfo()
        {
            return $"{Name} (Салат, {Quantitys} ккал) - {Price} грн, Заправка: {Dressing}";
        }

    // Генерує приклад Accessory
    public new static Accessory GenerateSample()
        {
            var rnd = Random.Shared;
            var dressings = new[] { "Olive", "Yogurt", "Vinaigrette", "Caesar" };
            var quantitys = rnd.Next(120, 600);
            var price = Math.Round(rnd.NextDouble() * 200 + 30, 2);
            return new Accessory($"Accessory {rnd.Next(100,999)}", price, rnd.Next(0,2) == 1, quantitys, dressings[rnd.Next(dressings.Length)]);
        }

        // CreateNew wrapper
        public new static Accessory CreateNew() => GenerateSample();
    }
}
