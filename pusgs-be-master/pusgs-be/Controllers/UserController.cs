using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pusgs_be.Dto;
using pusgs_be.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace pusgs_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISMTPMail _SMTPMail;

        public UserController(IUserService userService, ISMTPMail smtpMail)
        {
            _userService = userService;
            _SMTPMail = smtpMail;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto user)
        {
            try
            {
                JwtToken token = _userService.Login(user);
                return Ok(token);
            }
            catch (Exception e)
            {
                if (e.Message == "Email or password is incorrect!")
                {
                    return StatusCode(403, e.Message);
                }
                else if (e.Message == "Email not found!")
                {
                    return StatusCode(404, e.Message);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto user)
        {
            try
            {
                _userService.Register(user);

                return StatusCode(201);
            }
            catch (Exception e)
            {
                if (e.Message == "Email already exists!")
                {
                    return StatusCode(409, e.Message);
                }
                else if (e.Message == "Username already taken!")
                {
                    return StatusCode(409, e.Message);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetUsers()
        {
            try
            {
                return Ok(_userService.GetUsers());
            }
            catch
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }



        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                UserDto response = _userService.GetUser(id);
                if (response != null)
                    return Ok(response);
                else
                    return StatusCode(404, "User not found!");
            }
            catch
            {
                return StatusCode(500, "Internal Server Error!");
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto user)
        {
            try
            {
                _userService.Update(id, user);
                return Ok();
            }
            catch (Exception e)
            {
                if (e.Message == "User not found!")
                {
                    return StatusCode(404, e.Message);
                } 
                else if (e.Message == "Username " + user.Username + " is already taken!")
                {
                    return StatusCode(409, e.Message);
                }
                else if (e.Message == "Username already taken!")
                {
                    return StatusCode(409, e.Message);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult AppoveUser(int id)
        {
            try
            {
                _userService.Approve(id);
                return Ok();
            }
            catch (Exception e)
            {
                if (e.Message == "User not found!")
                {
                    return StatusCode(404, e.Message);
                } else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult RejectUser(int id)
        {
            try
            {
                _userService.Reject(id);
                return Ok();
            }
            catch (Exception e)
            {
                if (e.Message == "User not found!")
                {
                    return StatusCode(404, e.Message);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);

                return StatusCode(200);
            }
            catch (Exception e)
            {
                if (e.Message == "User not found!")
                {
                    return StatusCode(404, e.Message);
                }
                else
                {
                    return StatusCode(500, "Internal Server Error!");
                }
            }
        }

        [HttpPost]
        [Route("ng")]
        public IActionResult SendEmail()
        {
            _SMTPMail.SendMail("nebojsa.vasic997@gmail.com", "Search report", "You'll find the attachment below");
            return Ok();
        }
    }
}
