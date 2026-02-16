using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_website.Models
{
    public class Product
    {
        [Key]
        public int product_id { get; set; }

        [Required]
        public string product_name { get; set; }

        [Required]
        public decimal  product_price { get; set; }

        [Required]
        public string product_description { get; set; }

        [ValidateNever]
        public string product_image { get; set; }

        [Required]
        public int category_id { get; set; }

        [ValidateNever]
        public Category category { get; set; }  // Foreign key 

        public bool is_top_product { get; set; }

        public bool show_in_experience { get; set; } = false;

        // ===== Experience Page Fields =====

        public string? ExperienceType { get; set; }  // Gaming, WorkFromHome, SmartLiving, Entertainment

        public int ?ExperienceOrder { get; set; }     // Section me product ka order
    }
}
