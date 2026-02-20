using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZooShop.Infrastructure.Models;

[Table("PetFoods")]
public class PetFoodModel : ProductModel
{
    [Required]
    [Range(0.1, 50, ErrorMessage = "Вага має бути від 0.1 до 50 кг")]
    public double WeightKg { get; set; }

    [Required]
    [MaxLength(50)]
    public string AnimalType { get; set; } = string.Empty;

    [Required]
    public bool IsDryFood { get; set; }

    [MaxLength(300)]
    public string Composition { get; set; } = string.Empty;
}
