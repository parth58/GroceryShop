using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroceryShop.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public DateTime Datetime { get; set; }
        public string Status { get; set; }
        public string PaymentType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }



    }
}