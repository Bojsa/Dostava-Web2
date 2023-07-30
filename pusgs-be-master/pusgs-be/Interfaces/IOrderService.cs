using pusgs_be.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Interfaces
{
    public interface IOrderService
    {
        void AddOrder(OrderAddDto order);
        List<OrderDto> GetOrders();
        OrderDto GetOrder(int id);
        void DeleteOrder(int id);
        void TakeOrder(int userId, int id);
        void FinishOrder(int userId, int id);
    }
}
