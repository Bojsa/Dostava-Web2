using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using pusgs_be.Dto;
using pusgs_be.Infrastructure;
using pusgs_be.Interfaces;
using pusgs_be.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace pusgs_be.Services
{
    public class UserService : IUserService
    {
        private readonly IConfigurationSection _secretKey;
        private readonly IMapper _mapper;
        private readonly ISMTPMail _SMTPMail;
        private readonly DataContext _dbContext;

        public UserService(IConfiguration config, IMapper mapper, DataContext dbContext, ISMTPMail smtpMail)
        {
            _secretKey = config.GetSection("SecretKey");
            _mapper = mapper;
            _dbContext = dbContext;
            _SMTPMail = smtpMail;
        }

        public JwtToken Login(UserLoginDto userDto)
        {
            User user = _dbContext.Users.FirstOrDefault(x => x.Email == userDto.Email);

            if(user == null)
            {
                throw new Exception("Email not found!");
            }
            else if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                throw new Exception("Email or password is incorrect!");
            }

            List<Claim> claims = new();     //dodaje se u token
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            switch (user.Type)
            {
                case UserType.Administrator:
                    claims.Add(new Claim(ClaimTypes.Role, UserType.Administrator.ToString()));
                    break;
                case UserType.Deliverer:
                    claims.Add(new Claim(ClaimTypes.Role, UserType.Deliverer.ToString()));
                    break;
                case UserType.Customer:
                    claims.Add(new Claim(ClaimTypes.Role, UserType.Customer.ToString()));
                    break;
            }

            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:44375",
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: signinCredentials
            );
            // string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return
                 new JwtToken
                 {
                     Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
                     Expiration = tokenOptions.ValidTo,
                     UserId = tokenOptions.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                 };
        
        }
        

        public void Register(UserRegisterDto userDto)
        {
            if (_dbContext.Users.Any(x => x.Email == userDto.Email))
                throw new Exception("Email already exists!");

            if (_dbContext.Users.Any(x => x.Username == userDto.Username))
                throw new Exception("Username already taken!");

            User user = _mapper.Map<User>(userDto);
            if (user.Type == UserType.Administrator || user.Type == UserType.Customer)
                user.IsApproved = UserApprove.Approved;

            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public List<UserDto> GetUsers()
        {
            return _mapper.Map<List<UserDto>>(_dbContext.Users.ToList());
        }
        public List<UserDto> GetDeliverers()
        {
            return _mapper.Map<List<UserDto>>(_dbContext.Users.Select(x => x.Type == UserType.Deliverer).ToList());
        }

        public UserDto GetUser(int id)
        {
            return _mapper.Map<UserDto>(_dbContext.Users.Find(id));
        }

        public void Update(int id, UserUpdateDto userDto)
        {
            User user = _dbContext.Users.Find(id);
            if (user == null)
                throw new Exception("User not found!");

            if (!string.IsNullOrEmpty(userDto.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            if (user.Username != userDto.Username)
            {
                if (_dbContext.Users.Any(x => x.Username == userDto.Username))
                    throw new Exception("Username already taken!");

                user.Username = userDto.Username;
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Address = userDto.Address;
            _dbContext.SaveChanges();
        }

        public void Approve(int id)
        {
            User user = _dbContext.Users.Find(id);
            if (user == null)
                throw new Exception("User not found!");

            user.IsApproved = UserApprove.Approved;
            _SMTPMail.SendMail($"{user.Email}", "Verification report", $"Your request for verification has been Approved.");
            _dbContext.SaveChanges();
        }

        public void Reject(int id)
        {
            User user = _dbContext.Users.Find(id);
            if (user == null)
                throw new Exception("User not found!");

            user.IsApproved = UserApprove.Denied;
            _SMTPMail.SendMail($"{user.Email}", "Verification report", $"Your request for verification has been Denied.");
            _dbContext.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            User user = _dbContext.Users.Find(id);
            if (user == null)
                throw new Exception("User not found!");

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}
