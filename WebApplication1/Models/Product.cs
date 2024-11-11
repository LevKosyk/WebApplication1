using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Введіть від 2 до 20 символів!")]
        [Required(ErrorMessage = "Filed must be not empty")]
        public string Name { get; set; }
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        [Required(ErrorMessage = "Filed must be not empty")]
        [Precision(10, 2)]
        [Range(1, 100000)]
        public decimal Price { get; set; }
        [StringLength(1025, MinimumLength = 2, ErrorMessage = "Введіть від 2 до 20 символів!")]
        [Required(ErrorMessage = "Filed must be not empty")]
        public string Description { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }  
        public virtual Order Order { get; set; }

    }
}
