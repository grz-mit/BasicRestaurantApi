using BasicRestaurantAPI.DTO;
using BasicRestaurantAPI.Entities;
using BasicRestaurantAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateUserDto user)
        {
            _service.CreateAccount(user);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto login)
        {
            string token = _service.GenerateJwt(login);
            return Ok(token);
        }
    }
}
