using BasicRestaurantAPI.Entities;
using BasicRestaurantAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        [Route("{restaurantId}/dish")]
        public ActionResult GetAll([FromRoute]int restaurantId)
        {
            var result = _dishService.GetAll(restaurantId);
            return Ok(result);
        }

        [HttpGet]
        [Route("dish/{dishId}")]
        public ActionResult GetById([FromRoute] int dishId)
        {
            var result = _dishService.GetById(dishId);
           
            return Ok(result);
        }

        [HttpPost]
        [Route("{restaurantId}/dish")]
        public ActionResult Create([FromRoute]int restaurantId, [FromBody]CreateDishDto createDishDto)
        {
            _dishService.Create(restaurantId, createDishDto);
            
            return Ok();
        }

        [HttpDelete]
        [Route("{restaurantId}/dish")]
        public ActionResult Delete([FromRoute]int restaurantId)
        {
            _dishService.DeleteDishesFromRestaurant(restaurantId);

            return NoContent();
        }


    }
}
