using pusgs_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public float Price { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsTaken { get; set; }
        public int? DelivererId { get; set; }
        public User Deliverer { get; set; }
        public List<OrderProductAddDto> Products { get; set; }
    }
}
