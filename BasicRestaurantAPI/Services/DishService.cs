using BasicRestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BasicRestaurantAPI.Exceptions;
using AutoMapper;

namespace BasicRestaurantAPI.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext restaurantDbContext, IMapper mapper)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
        }

        public List<Dish> GetAll(int restaurantId)
        {
            var dishes = _restaurantDbContext.Dishes.Where(d => d.RestaurantId == restaurantId).ToList();

            if (dishes is null)
            {
                throw new NotFoundException("Restaurant or dish not found");
            }

            return dishes;
        }

        public Dish GetById(int dishId)
        {
            var dish = _restaurantDbContext.Dishes.FirstOrDefault(d=>d.Id == dishId);

            if (dish is null)
            {
                throw new NotFoundException("Restaurant or dish not found");
            }

            return dish;
        }

        public void DeleteDishesFromRestaurant(int restaurantId)
        {
            var dishes = _restaurantDbContext.Dishes.Where(d => d.RestaurantId == restaurantId).ToList();

            if (dishes is null)
            {
                throw new NotFoundException("Restaurant or dish not found");
            }

            _restaurantDbContext.Remove(dishes);
            _restaurantDbContext.SaveChanges();
        }

        public void Create(int restaurantId, CreateDishDto createDishDto)
        {
            var restaurant = _restaurantDbContext.Restaurants.Include(d=>d.Dishes)
                                                             .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant or dish not found");
            }

            restaurant.Dishes.Add(_mapper.Map<Dish>(createDishDto));

            _restaurantDbContext.Restaurants.Update(restaurant);
            
            _restaurantDbContext.SaveChanges();


        }
    }
}
