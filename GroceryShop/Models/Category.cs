using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroceryShop.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public virtual List<Product> Products { get; set; }

    }
}