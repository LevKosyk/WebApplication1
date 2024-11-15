using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public ApiOrderController(IServiceOrder serviceOrder)
        {
            _serviceOrder = serviceOrder;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (ServiceUser.Order == null || ServiceUser.Order.Id == 0)
            {
                return BadRequest(new { message = "Order not initialized for the user" });
            }

            var products = await _serviceOrder.GetProductsAsync(ServiceUser.Order.Id);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _serviceOrder.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            return Ok(order);
        }
        [HttpPost("create")]
        public async Task<ActionResult<Order>> CreateOrder()
        {
            var newOrder = await _serviceOrder.CreateOrder();
            if (newOrder == null)
            {
                return StatusCode(500, new { message = "Failed to create the order" });
            }

            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, newOrder);
        }

        [HttpPut("add-product")]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest(new { message = "Product object is null" });
            }

            if (ServiceUser.Order == null || ServiceUser.Order.Id == 0)
            {
                return BadRequest(new { message = "Order not initialized for the user" });
            }

            var addedProduct = await _serviceOrder.AddProductAsync(product, ServiceUser.Order.Id);
            if (addedProduct == null)
            {
                return StatusCode(500, new { message = "Failed to add the product to the order" });
            }

            return Ok(addedProduct);
        }
        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (ServiceUser.Order == null || ServiceUser.Order.Id == 0)
            {
                return BadRequest(new { message = "Order not initialized for the user" });
            }

            var deleted = await _serviceOrder.DeleteProductAsync(id, ServiceUser.Order.Id);
            if (!deleted)
            {
                return NotFound(new { message = "Product not found in the order" });
            }

            return Ok(new { message = "Product deleted successfully" });
        }
    }
}
