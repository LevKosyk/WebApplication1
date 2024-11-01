using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
