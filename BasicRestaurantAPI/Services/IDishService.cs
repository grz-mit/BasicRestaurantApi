using BasicRestaurantAPI.Entities;
using System.Collections.Generic;

namespace BasicRestaurantAPI.Services
{
    public interface IDishService
    {
        void Create(int restaurantId, CreateDishDto createDishDto);
        void DeleteDishesFromRestaurant(int restaurantId);
        List<Dish> GetAll(int restaurantId);
        Dish GetById(int restaurantId, int dishId);
    }
}