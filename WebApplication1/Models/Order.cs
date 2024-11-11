using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
