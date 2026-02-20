using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZooShop.Infrastructure.Models
{
    // Базовий клас для Table-per-Type (TPT) підходу
    [Table("Productes")]
    public class ProductModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Назва страви для тварин обов'язкова")]
        [MaxLength(100, ErrorMessage = "Назва не може перевищувати 100 символів")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ціна має бути більше нуля")]
        [Column(TypeName = "decimal(18,2)")]
        public double Price { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        // Зв'язок багато-до-багатьох з OrderModel через OrderProductModel
        public ICollection<OrderProductModel> OrderProductes { get; set; } = new List<OrderProductModel>();
    }
}

