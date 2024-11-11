using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiOrderController : ControllerBase
    {
        private readonly IServiceOrder _serviceOrder;

        public ApiOrderController(IServiceOrder context)
        {
            _serviceOrder = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetProducts()
        {
            var orders = await _serviceOrder.GetProductsAsync(ServiceUser.Order.Id);
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetProduct(int id)
        {
            var order = await _serviceOrder.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder()
        {
            _ = await _serviceOrder.CreateOrder();
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult<Order>> AddProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Product object is null");
            }
            _ = await _serviceOrder.AddProductAsync(product, ServiceUser.Order.Id);
            return Ok(product);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _serviceOrder.DeleteProductAsync(id, ServiceUser.Order.Id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok(new { message = "Product deleted successfully ..." });
        }
    }
}
