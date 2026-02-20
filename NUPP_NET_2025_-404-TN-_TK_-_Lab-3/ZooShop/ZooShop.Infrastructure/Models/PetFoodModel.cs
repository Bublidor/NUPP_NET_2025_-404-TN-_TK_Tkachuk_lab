using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZooShop.Infrastructure.Models
{
    // Table-per-Type (TPT) - окрема таблиця для PetFood
    [Table("PetProducts")]
    public class PetFoodModel : ProductModel
    {
        [Required]
        [Range(20, 50, ErrorMessage = "Розмір їжі має бути від 20 до 50 см")]
        public int SizeCm { get; set; }

        [Required]
        [MaxLength(50)]
        public string DoughType { get; set; } = string.Empty;

        public bool ExtraCheese { get; set; }

        [MaxLength(200)]
        public string Toppings { get; set; } = string.Empty;
    }
}

