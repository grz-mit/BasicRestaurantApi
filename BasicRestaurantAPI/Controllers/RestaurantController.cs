using AutoMapper;
using BasicRestaurantAPI.DTO;
using BasicRestaurantAPI.Entities;
using BasicRestaurantAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public ActionResult Edit ([FromRoute] int id, [FromBody] EditRestaurantDto editRestaurantDto)
        {
            _restaurantService.Edit(id, editRestaurantDto);

            return Ok();
        }

        
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);

            return NoContent();
        }


        [HttpPost]
        [Authorize]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto restaurantDto)
        {
            var newRestaurantId = _restaurantService.CreateRestaurant(restaurantDto);
            
            return Created($"/api/restaurant/{newRestaurantId}", null);
        }

        [HttpGet]
        [Authorize(Policy = "HasNationality")]
        [Authorize(Policy = "Atleast18")]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var restaurantsDtos = _restaurantService.GetAllRestaurants();
            
            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> GetRestaurant([FromRoute] int id)
        {
            var restaurantDto = _restaurantService.GetRestaurantById(id);

            return Ok(restaurantDto);
        }
    }
}
