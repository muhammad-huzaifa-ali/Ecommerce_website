using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_website.Models
{
    public class Cart
    {
        [Key]
        public int cart_id { get; set; }

        [Required]
        public int product_id { get; set; }

        [Required]
        public int customer_id { get; set; }

        [Required]
        public int product_quantity { get; set; }

        [Required]
        public int product_status  { get; set; }


        [ForeignKey("product_id")]
        public Product product { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }


    }
}
