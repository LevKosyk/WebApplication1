﻿using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IServiceProduct
    {
        Task<Product> CreateAsync (Product product);
        Task<IEnumerable<Product>> ReadAsync();
        Task<Product> GetByIdAsync(int Id);
        Task<Product> UpdateAsync (int id, Product product);
        Task<bool> DeleteAsync (int id);

    }
    public class ServiceProduct : IServiceProduct
    {
        private readonly ProductContext _productContext;
        private readonly ILogger<ServiceProduct> _logger;
        public ServiceProduct(ProductContext productContext, ILogger<ServiceProduct> logger)
        {
            _productContext = productContext;
            _logger = logger;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            if (product == null)
            {
                _logger.LogWarning("Attemt null");
                return null;
            }
            await _productContext.AddAsync(product);
            Console.WriteLine(product.Name, product.Description);
            await _productContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Product product = await _productContext.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            _productContext.Products.Remove(product);   
            await _productContext.SaveChangesAsync();
            return true;
        }

        public async Task<Product> GetByIdAsync(int Id)
        {
            return await _productContext.Products.FindAsync(Id);
        }

        public async Task<IEnumerable<Product>> ReadAsync()
        {
            return await _productContext.Products.ToListAsync();
        }

        public async Task<Product> UpdateAsync(int id, Product product)
        {
            if (product == null || id != product.Id)
            {
                _logger.LogWarning("Error");
                return null;
            }
            try
            {
                _productContext.Products.Update(product);
                await _productContext.SaveChangesAsync();
                return product;
            }
            catch (DbUpdateConcurrencyException ex) {
                _logger.LogError(ex.Message);
                return null;
            }
            

        }
    }
}
