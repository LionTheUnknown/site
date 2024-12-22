using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double OriginalPrice { get; set; }
        public int Quantity { get; set; }
    }
}