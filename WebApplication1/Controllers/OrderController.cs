using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        private readonly IServiceOrder _serviceProduct;
        private readonly UserManager<IdentityUser> _userManager;
        public OrderController(IServiceOrder serviceProduct, UserManager<IdentityUser> userManager)
        {
            _serviceProduct = serviceProduct;
            _userManager = userManager;
        }
        public async Task<IActionResult> AddProduct(Product product)
        {
            var userId = _userManager.GetUserId(User);
            await _serviceProduct.AddProductAsync(product, int.Parse(userId));
            return View(product);
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _serviceProduct.DeleteProductAsync(id, int.Parse(userId));
            return View(result);
        }
        public async Task<IActionResult> GetProducts()
        {
            var userId = _userManager.GetUserId(User);
            return View(await _serviceProduct.GetProductsAsync(int.Parse(userId)));
        }
        public async Task<IActionResult> GetProductById(int id)
        {
            return View(await _serviceProduct.GetByIdAsync(id));
        }
    }
}
