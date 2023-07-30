using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Components { get; set; }
        public float Price { get; set; }
        public List<OrderProduct> Orders { get; set; }
    }
}
