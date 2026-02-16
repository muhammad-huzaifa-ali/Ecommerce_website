using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_website.Models
{
    public class Category
    {
        [Key]
        public int category_id { get; set; }

        [Required]
        public string category_name { get; set; }

        [ValidateNever]
        public  List <Product> product {get;set;}
    }

}
