using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IServiceOrder
    {
        Task<Product> AddProductAsync(Product product, int orderId);
        Task<IEnumerable<Product>> GetProductsAsync(int orderId);
        Task<Product> GetByIdAsync(int id);
        Task<bool> DeleteProductAsync(int id, int orderId);
    }

    public class ServiceOrder : IServiceOrder
    {
        private readonly ProductContext _productContext;
        private readonly ILogger<ServiceOrder> _logger;

        public ServiceOrder(ProductContext productContext, ILogger<ServiceOrder> logger)
        {
            _productContext = productContext;
            _logger = logger;
        }

        public async Task<Product> AddProductAsync(Product product, int orderId)
        {
            if (product == null)
            {
                _logger.LogWarning("Attempt to add a null product");
                return null;
            }

            var order = await _productContext.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                _logger.LogWarning($"Order with ID {orderId} not found");
                return null;
            }

            order.Products.Add(product);
            await _productContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id, int orderId)
        {
            var order = await _productContext.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                _logger.LogWarning($"Order with ID {orderId} not found");
                return false;
            }

            var product = order.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found in Order {orderId}");
                return false;
            }

            order.Products.Remove(product);
            await _productContext.SaveChangesAsync();
            return true;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productContext.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int orderId)
        {
            var order = await _productContext.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == orderId);
            return order?.Products ?? new List<Product>();
        }
    }
}
