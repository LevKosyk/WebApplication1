using Microsoft.EntityFrameworkCore;
using WebApplication1.Models; // Импортирует пространство имен для работы с Entity Framework Core // Импортирует пространство имен, где находятся модели приложения

// Контекст базы данных для продуктов, наследующий от DbContext
public class ProductContext : DbContext
{
    // Конструктор принимает параметры конфигурации контекста базы данных
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options) // Передает параметры в базовый класс DbContext
    {
        // Базовый класс DbContext настроит соединение с базой данных и определит таблицы
    }

    // Свойство для доступа к таблице продуктов в базе данных
    public DbSet<Product> Products { get; set; }
}