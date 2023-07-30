using AutoMapper;
using pusgs_be.Dto;
using pusgs_be.Infrastructure;
using pusgs_be.Interfaces;
using pusgs_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public OrderService(IMapper mapper, DataContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public void AddOrder(OrderAddDto order)
        {
            Order o = _mapper.Map<Order>(order);
            _dbContext.Orders.Add(o);
            _dbContext.SaveChanges();

            foreach(OrderProductAddDto x in order.Products)
            {
                OrderProduct op = new()
                {
                    Order = o,
                    OrderId = o.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                };

                _dbContext.OrderProducts.Add(op);
                _dbContext.SaveChanges();
            }

        }

        public void DeleteOrder(int id)
        {
            Order o = _dbContext.Orders.Find(id);
            if (o == null)
                throw new Exception("Order not found!");

            _dbContext.Orders.Remove(o);
            _dbContext.SaveChanges();
        }

        public OrderDto GetOrder(int id)
        {
            OrderDto order = _mapper.Map<OrderDto>(_dbContext.Orders.Find(id));
            if (order == null)
                throw new Exception("Order not found!");

            order.Products = _mapper.Map<List<OrderProductAddDto>>(_dbContext.OrderProducts.Where(c => c.OrderId == id).ToList());

            return order;
        }

        public List<OrderDto> GetOrders()
        {
            return _mapper.Map<List<OrderDto>>(_dbContext.Orders.ToList());
        }

        public void TakeOrder(int userId, int id)
        {
            User user = _dbContext.Users.Find(userId);
            if (user == null)
                throw new Exception("User not found!");

            if (user.IsApproved != UserApprove.Approved)
                throw new Exception("User is not approved!");

            Order o = _dbContext.Orders.Find(id);
            if (o == null)
                throw new Exception("Order not found!");

            foreach(var order in _dbContext.Orders)
            {
                if (order.DelivererId == userId && order.Status == OrderStatus.Delivering)
                {
                    throw new Exception("User has unfisnished order!");
                }
            }

            if (!o.IsTaken && o.Status == OrderStatus.Pending)
            {
                o.Status = OrderStatus.Delivering;
                o.IsTaken = true;
                o.DelivererId = userId;

                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Order already taken!");
            }
        }

        public void FinishOrder(int userId, int id)
        {
            User user = _dbContext.Users.Find(userId);
            if (user == null)
                throw new Exception("User not found!");

            if (user.IsApproved != UserApprove.Approved)
                throw new Exception("User is not approved!");

            Order o = _dbContext.Orders.Find(id);
            if (o == null)
                throw new Exception("Order not found!");

            if (o.IsTaken && o.Status == OrderStatus.Delivering && o.DelivererId == userId)
            {
                o.Status = OrderStatus.Delivered;

                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Internal Server Error!");
            }
        }
    }
}
