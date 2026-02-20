using ZooShop.Infrastructure.Models;

namespace ZooShop.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторій для роботи зі стравами
    /// </summary>
    public interface IProductRepository : IRepository<ProductModel>
    {
        /// <summary>
        /// Отримати всі доступні страви
        /// </summary>
        Task<IEnumerable<ProductModel>> GetAvailableProductesAsync();

        /// <summary>
        /// Отримати страви за ціновим діапазоном
        /// </summary>
        Task<IEnumerable<ProductModel>> GetProductesByPriceRangeAsync(double minPrice, double maxPrice);

        /// <summary>
        /// Отримати страву за назвою
        /// </summary>
        Task<ProductModel?> GetByNameAsync(string name);

        /// <summary>
        /// Отримати всі піци
        /// </summary>
        Task<IEnumerable<PetFoodModel>> GetAllPetProductsAsync();

        /// <summary>
        /// Отримати всі салати
        /// </summary>
        Task<IEnumerable<AccessoryModel>> GetAllAccessorysAsync();

        /// <summary>
        /// Отримати вегетаріанські салати
        /// </summary>
        Task<IEnumerable<AccessoryModel>> GetVegetarianAccessorysAsync();
    }
}

