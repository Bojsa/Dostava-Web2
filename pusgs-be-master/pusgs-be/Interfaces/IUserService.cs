using pusgs_be.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Interfaces
{
    public interface IUserService
    {
        JwtToken Login(UserLoginDto user);
        void Register(UserRegisterDto user);
        void Update(int id, UserUpdateDto user);
        void Approve(int id);
        void Reject(int id);
        List<UserDto> GetUsers();
        List<UserDto> GetDeliverers();
        UserDto GetUser(int id);
        void DeleteUser(int id);
    }
}
