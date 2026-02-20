using Microsoft.EntityFrameworkCore;
using ZooShop.Infrastructure.Models;

namespace ZooShop.Infrastructure.Repositories
{
    /// <summary>
    /// Реалізація репозиторію для роботи зі стравами
    /// </summary>
    public class ProductRepository : Repository<ProductModel>, IProductRepository
    {
        public ProductRepository(ZooShopContext context) : base(context)
        {
        }

        /// <summary>
        /// Отримати всі доступні страви
        /// </summary>
        public async Task<IEnumerable<ProductModel>> GetAvailableProductesAsync()
        {
            return await _dbSet
                .Where(d => d.IsAvailable)
                .ToListAsync();
        }

        /// <summary>
        /// Отримати страви за ціновим діапазоном
        /// </summary>
        public async Task<IEnumerable<ProductModel>> GetProductesByPriceRangeAsync(double minPrice, double maxPrice)
        {
            return await _dbSet
                .Where(d => d.Price >= minPrice && d.Price <= maxPrice)
                .OrderBy(d => d.Price)
                .ToListAsync();
        }

        /// <summary>
        /// Отримати страву за назвою
        /// </summary>
        public async Task<ProductModel?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(d => d.Name == name);
        }

        /// <summary>
        /// Отримати всі піци
        /// </summary>
        public async Task<IEnumerable<PetFoodModel>> GetAllPetProductsAsync()
        {
            return await _context.PetProducts
                .Where(p => p.IsAvailable)
                .ToListAsync();
        }

        /// <summary>
        /// Отримати всі салати
        /// </summary>
        public async Task<IEnumerable<AccessoryModel>> GetAllAccessorysAsync()
        {
            return await _context.Accessorys
                .Where(s => s.IsAvailable)
                .ToListAsync();
        }

        /// <summary>
        /// Отримати вегетаріанські салати
        /// </summary>
        public async Task<IEnumerable<AccessoryModel>> GetVegetarianAccessorysAsync()
        {
            return await _context.Accessorys
                .Where(s => s.IsVegetarian && s.IsAvailable)
                .ToListAsync();
        }
    }
}

