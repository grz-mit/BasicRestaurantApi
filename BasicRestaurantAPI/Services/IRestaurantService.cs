using BasicRestaurantAPI.DTO;
using BasicRestaurantAPI.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace BasicRestaurantAPI.Services
{
    public interface IRestaurantService
    {
        int CreateRestaurant(CreateRestaurantDto restaurantDto);
        List<RestaurantDto> GetAllRestaurants();
        RestaurantDto GetRestaurantById(int id);
        void Delete(int id);
        void Edit(int id, EditRestaurantDto editRestaurantDto);
    }
}