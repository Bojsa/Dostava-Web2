using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Dto
{
    public class OrderAddDto
    {
        public int UserId { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public float Price { get; set; }
        public List<OrderProductAddDto> Products { get; set; }
    }
}
