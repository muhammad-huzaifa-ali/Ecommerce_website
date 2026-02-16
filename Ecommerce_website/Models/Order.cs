using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_website.Models
{
    public class Order
    {
      
        [Key]
        public int order_id { get; set; }

        public int product_id { get; set; }

        public int customer_id { get; set; }

        public int quantity { get; set; }

        public DateTime order_date { get; set; }

        //  Navigation properties

        [ForeignKey("product_id")]
        public Product product { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }


    }
}

