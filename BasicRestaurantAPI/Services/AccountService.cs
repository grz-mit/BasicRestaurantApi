using AutoMapper;
using BasicRestaurantAPI.DTO;
using BasicRestaurantAPI.Entities;
using BasicRestaurantAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthSettings _authSettings;

        public AccountService(RestaurantDbContext context, IMapper mapper, IPasswordHasher<User> passwordHasher, AuthSettings authSettings)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authSettings = authSettings;
        }

        public void CreateAccount(CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password); ;

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public string GenerateJwt(LoginDto loginDto)
        {
            var user = _context.Users
                               .Include(u=> u.Role)
                               .FirstOrDefault(u => u.Email == loginDto.Email);
                                    
            if(user == null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadImageFormatException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),   
            };

            if(!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(new Claim("Nationality", user.Nationality));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDate = DateTime.Now.AddDays(_authSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                _authSettings.JwtIssuer,
                _authSettings.JwtIssuer,
                claims,
                expires: expireDate,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
