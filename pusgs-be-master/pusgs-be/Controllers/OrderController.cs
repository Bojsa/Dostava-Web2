using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pusgs_be.Dto;
using pusgs_be.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pusgs_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            try
            {
                return Ok(_orderService.GetOrders());
            }
            catch
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            try
            {
                return Ok(_orderService.GetOrder(id));
            }
            catch (Exception e)
            {
                if (e.Message == "Order not found!")
                    return StatusCode(404, "Order not found!");
                else
                    return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IActionResult AddOrder([FromBody] OrderAddDto order)
        {
            try
            {
                var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                _orderService.AddOrder(order);

                return StatusCode(201);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                _orderService.DeleteOrder(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                if (e.Message == "Order not found!")
                {
                    return StatusCode(404, e.Message);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }

        [HttpPut("{userId}/take/{orderId}")]
        [Authorize(Roles = "Deliverer")]
        public IActionResult TakeOrder(int userId, int orderId)
        {
            try
            {
                _orderService.TakeOrder(userId, orderId); // change user id

                return Ok();
            }
            catch (Exception e)
            {
                if (e.Message == "User not found!")
                {
                    return StatusCode(404, e.Message);
                }
                else if (e.Message == "User is not approved!")
                {
                    return StatusCode(404, e.Message); // change error code
                }
                else if (e.Message == "Order not found!")
                {
                    return StatusCode(404, e.Message);
                }
                else if (e.Message == "Order already taken!")
                {
                    return StatusCode(409, e.Message); // change error code
                }
                else if (e.Message == "User has unfisnished order!")
                {
                    return StatusCode(409, e.Message);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }

        [HttpPut("{id}/finish")]
        [Authorize(Roles = "Deliverer")]
        public IActionResult FinishOrder(int id)
        {
            try
            {
                var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                _orderService.FinishOrder(Int32.Parse(userId), id); // change user id

                return Ok();
            }
            catch (Exception e)
            {
                if (e.Message == "User not found!")
                {
                    return StatusCode(404, e.Message);
                }
                else if (e.Message == "User is not approved!")
                {
                    return StatusCode(404, e.Message); // change error code
                }
                else if (e.Message == "Order not found!")
                {
                    return StatusCode(404, e.Message);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }
    }
}
