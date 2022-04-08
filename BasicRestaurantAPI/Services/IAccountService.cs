using BasicRestaurantAPI.DTO;
using BasicRestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Services
{
    public interface IAccountService
    {
        public void CreateAccount(CreateUserDto user);
        public string GenerateJwt(LoginDto loginDto);
    }
}
