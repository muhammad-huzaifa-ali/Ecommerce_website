using System.ComponentModel.DataAnnotations;

namespace Ecommerce_website.Models
{
    public class Feedback
    {
        [Key]
        public int feedback_id { get; set; }

        [Required]
        public string customer_name { get; set; }
        [Required]
        public int customer_id { get; set; }

        [Required]
        public string customer_email { get; set; }

        [Required]
        public string customer_message { get; set; }
    }
}
