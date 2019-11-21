using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroceryShop.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int stock { get; set; }
        [Display(Name = "Image")]
        public string ImageURL { get; set; }
        public string Tags { get; set; }
        public virtual Category Category { get; set; }
      
    }
}