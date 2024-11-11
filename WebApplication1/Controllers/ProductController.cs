using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private readonly IServiceProduct _serviceProduct;
        public ProductController(IServiceProduct serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }
        [HttpGet]
        public async Task<IActionResult> Read()
        {
            var products = await _serviceProduct.ReadAsync();
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await  _serviceProduct.GetByIdAsync(id);
            return View(product);
        }
        [HttpGet]
        public IActionResult Create() {

            if (User.IsInRole("admin")|| User.IsInRole("moderator"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Page404");
            }
        }
        [Authorize(Roles = "admin,moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Product product)
        {
            await _serviceProduct.CreateAsync(product);
            return RedirectToAction("Read");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (User.IsInRole("admin") || User.IsInRole("moderator"))
            {
                Product product = await _serviceProduct.GetByIdAsync(id);
                return View(product);
            }
            else
            {
                return RedirectToAction("Page404");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Product product)
        {

            Console.WriteLine(id);
            if (ModelState.IsValid)
            {
                
                _ = await _serviceProduct.UpdateAsync(id, product);
                return RedirectToAction("Read");
            }
            return View("Read");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.IsInRole("admin") || User.IsInRole("moderator"))
            {
                Product product = await _serviceProduct.GetByIdAsync(id);
                return View(product);
            }
            else
            {
                return RedirectToAction("Page404");
            }

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, string name)
        {
            Console.WriteLine("id");
            bool result = await _serviceProduct.DeleteAsync(id);
            if (result)
            {
                return RedirectToAction("Read");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult Page404()
        {
            return View();
        }
    }
}
