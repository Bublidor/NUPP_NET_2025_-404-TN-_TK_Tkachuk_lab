using Microsoft.EntityFrameworkCore;
using ZooShop.Infrastructure.Models;

namespace ZooShop.Infrastructure
{
    /// <summary>
    /// Контекст бази даних для ресторану
    /// Описує схему бази даних з використанням Entity Framework та анотацій
    /// </summary>
    public class ZooShopContext : DbContext
    {
        // DbSet для всіх таблиць бази даних
        public DbSet<CustomerModel> Customers { get; set; } = null!;
        public DbSet<CustomerProfileModel> CustomerProfiles { get; set; } = null!;
        public DbSet<ProductModel> Productes { get; set; } = null!;
        public DbSet<PetFoodModel> PetProducts { get; set; } = null!;
        public DbSet<AccessoryModel> Accessorys { get; set; } = null!;
        public DbSet<OrderModel> Orders { get; set; } = null!;
        public DbSet<OrderProductModel> OrderProductes { get; set; } = null!;

        // Конструктор за замовчуванням
        public ZooShopContext()
        {
        }

        // Конструктор з параметрами для налаштування
        public ZooShopContext(DbContextOptions<ZooShopContext> options)
            : base(options)
        {
        }

        // Налаштування підключення до бази даних
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Підключення до PostgreSQL
                string connectionString = Environment.GetEnvironmentVariable("RESTAURANT_DB_CONNECTION")
                    ?? "Host=localhost;Port=5432;Database=ZooShopDB;Username=postgres;Password=h2olioh8co;Include Error Detail=true";

                optionsBuilder.UseNpgsql(
                    connectionString,
                    options => options.EnableRetryOnFailure());
            }
        }

        // Налаштування моделей та зв'язків через Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================================
            // Налаштування Table-per-Type (TPT) для наслідування
            // =====================================================

            // Базова таблиця Productes
            modelBuilder.Entity<ProductModel>()
                .ToTable("Productes")
                .HasKey(d => d.Id);

            // PetFood наслідується від Product (TPT)
            modelBuilder.Entity<PetFoodModel>()
                .ToTable("PetProducts")
                .HasBaseType<ProductModel>();

            // Accessory наслідується від Product (TPT)
            modelBuilder.Entity<AccessoryModel>()
                .ToTable("Accessorys")
                .HasBaseType<ProductModel>();

            // =====================================================
            // Налаштування зв'язку один-до-одного
            // CustomerModel <-> CustomerProfileModel
            // =====================================================

            modelBuilder.Entity<CustomerModel>()
                .HasOne(c => c.Profile)
                .WithOne(p => p.Customer)
                .HasForeignKey<CustomerProfileModel>(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================================================
            // Налаштування зв'язку один-до-багатьох
            // CustomerModel -> OrderModel
            // =====================================================

            modelBuilder.Entity<CustomerModel>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // =====================================================
            // Налаштування зв'язку багато-до-багатьох
            // OrderModel <-> ProductModel через OrderProductModel
            // =====================================================

            modelBuilder.Entity<OrderProductModel>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderProductes)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderProductModel>()
                .HasOne(od => od.Product)
                .WithMany(d => d.OrderProductes)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Індекс для унікальності пари Order-Product (опціонально)
            modelBuilder.Entity<OrderProductModel>()
                .HasIndex(od => new { od.OrderId, od.ProductId })
                .IsUnique(false); // Дозволяємо повторні записи (одна страва може бути кілька разів)

            // =====================================================
            // Додаткові налаштування для полів
            // =====================================================

            // Налаштування decimal для цін
            modelBuilder.Entity<ProductModel>()
                .Property(d => d.Price)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderModel>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderProductModel>()
                .Property(od => od.PriceAtOrder)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            // Налаштування значень за замовчуванням
            modelBuilder.Entity<OrderModel>()
                .Property(o => o.Status)
                .HasDefaultValue("Pending");

            modelBuilder.Entity<OrderModel>()
                .Property(o => o.OrderDate)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<ProductModel>()
                .Property(d => d.IsAvailable)
                .HasDefaultValue(true);

            // Індекси для покращення продуктивності
            modelBuilder.Entity<CustomerModel>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique(true);

            modelBuilder.Entity<CustomerProfileModel>()
                .HasIndex(cp => cp.Email)
                .IsUnique(true);

            modelBuilder.Entity<OrderModel>()
                .HasIndex(o => o.OrderDate);

            modelBuilder.Entity<OrderModel>()
                .HasIndex(o => o.Status);

            // =====================================================
            // Seed Data (початкові дані для тестування)
            // =====================================================

            SeedData(modelBuilder);
        }

        /// <summary>
        /// Метод для заповнення бази даних початковими даними
        /// </summary>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Статичні GUID для уникнення помилки PendingModelChangesWarning
            var customer1Id = new Guid("11111111-1111-1111-1111-111111111111");
            var customer2Id = new Guid("22222222-2222-2222-2222-222222222222");
            var profile1Id = new Guid("33333333-3333-3333-3333-333333333333");
            var profile2Id = new Guid("44444444-4444-4444-4444-444444444444");
            var pizza1Id = new Guid("55555555-5555-5555-5555-555555555555");
            var pizza2Id = new Guid("66666666-6666-6666-6666-666666666666");
            var salad1Id = new Guid("77777777-7777-7777-7777-777777777777");
            var salad2Id = new Guid("88888888-8888-8888-8888-888888888888");

            // Клієнти
            modelBuilder.Entity<CustomerModel>().HasData(
                new CustomerModel
                {
                    Id = customer1Id,
                    FullName = "Іван Петренко",
                    PhoneNumber = "+380501234567",
                    LoyaltyPoints = 100
                },
                new CustomerModel
                {
                    Id = customer2Id,
                    FullName = "Марія Коваленко",
                    PhoneNumber = "+380502345678",
                    LoyaltyPoints = 250
                }
            );

            // Профілі клієнтів
            modelBuilder.Entity<CustomerProfileModel>().HasData(
                new CustomerProfileModel
                {
                    Id = profile1Id,
                    CustomerId = customer1Id,
                    Email = "ivan.petrenko@example.com",
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(1990, 5, 15), DateTimeKind.Utc),
                    Address = "Київ, вул. Хрещатик, 1",
                    PreferredPaymentMethod = "Карта"
                },
                new CustomerProfileModel
                {
                    Id = profile2Id,
                    CustomerId = customer2Id,
                    Email = "maria.kovalenko@example.com",
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(1985, 8, 22), DateTimeKind.Utc),
                    Address = "Львів, пр. Свободи, 10",
                    PreferredPaymentMethod = "Готівка"
                }
            );

            // Піци
            modelBuilder.Entity<PetFoodModel>().HasData(
                new PetFoodModel
                {
                    Id = pizza1Id,
                    Name = "Маргарита",
                    Price = 180.00,
                    Description = "Класична піца з томатами та моцарелою",
                    IsAvailable = true,
                    SizeCm = 30,
                    DoughType = "Тонке тісто",
                    ExtraCheese = false,
                    Toppings = "Томати, моцарела, базилік"
                },
                new PetFoodModel
                {
                    Id = pizza2Id,
                    Name = "Пепероні",
                    Price = 220.00,
                    Description = "Піца з пепероні та сиром",
                    IsAvailable = true,
                    SizeCm = 35,
                    DoughType = "Традиційне тісто",
                    ExtraCheese = true,
                    Toppings = "Пепероні, моцарела, томатний соус"
                }
            );

            // Салати
            modelBuilder.Entity<AccessoryModel>().HasData(
                new AccessoryModel
                {
                    Id = salad1Id,
                    Name = "Цезар",
                    Price = 120.00,
                    Description = "Салат Цезар з куркою",
                    IsAvailable = true,
                    IsVegetarian = false,
                    Quantitys = 350,
                    Dressing = "Соус Цезар",
                    Ingredients = "Салат айсберг, курка, сир пармезан, крутони"
                },
                new AccessoryModel
                {
                    Id = salad2Id,
                    Name = "Грецький салат",
                    Price = 95.00,
                    Description = "Традиційний грецький салат",
                    IsAvailable = true,
                    IsVegetarian = true,
                    Quantitys = 180,
                    Dressing = "Оливкова олія",
                    Ingredients = "Огірки, томати, фета, оливки, цибуля"
                }
            );
        }
    }
}

