namespace ZooShop.Common
{
    public class PetFood : Product
    {
        public int SizeCm { get; set; }
        public string DoughType { get; set; }
        public bool ExtraCheese { get; set; }

        // конструктор
        public PetFood(string name, double price, int sizeCm, string doughType, bool extraCheese)
            : base(name, price)
        {
            SizeCm = sizeCm;
            DoughType = doughType;
            ExtraCheese = extraCheese;
        }

        public override string GetInfo()
        {
            return $"{Name} (Піца {SizeCm} см, тісто: {DoughType}, дод. сир: {ExtraCheese}) - {Price} грн";
        }

    // Генерує приклад PetFood
    public new static PetFood GenerateSample()
        {
            var rnd = Random.Shared;
            var sizes = new[] { 26, 30, 33, 40 };
            var doughs = new[] { "Thin", "Classic", "WholeWheat" };
            var size = sizes[rnd.Next(sizes.Length)];
            var dough = doughs[rnd.Next(doughs.Length)];
            var price = Math.Round(rnd.NextDouble() * 500 + 80, 2);
            return new PetFood($"PetFood {rnd.Next(100, 999)}", price, size, dough, rnd.Next(0, 2) == 1);
        }

        // CreateNew wrapper
        public new static PetFood CreateNew() => GenerateSample();
    }
}
