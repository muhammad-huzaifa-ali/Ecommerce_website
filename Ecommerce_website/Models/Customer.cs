using System.ComponentModel.DataAnnotations;

namespace Ecommerce_website.Models
{
    public class Customer
    {
        [Key]
        public int customer_id { get; set; }

        [Required]
        public string customer_name { get; set; }

        [Required]
        public string customer_phone { get; set; }

        [Required]
        public string customer_email { get; set; }

        [Required]
        public string customer_password { get; set; }

        public string ?customer_image { get; set; }
        [Required]
        public string customer_country { get; set; }

        [Required]
        public string customer_city { get; set; }

        [Required]
        public string customer_address { get; set; }

        [Required]
        public string customer_gender { get; set; }



    }
}
