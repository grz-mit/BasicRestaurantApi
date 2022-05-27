using AutoMapper;
using BasicRestaurantAPI.Authorization;
using BasicRestaurantAPI.DTO;
using BasicRestaurantAPI.Entities;
using BasicRestaurantAPI.Exceptions;
using BasicRestaurantAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        public RestaurantService(RestaurantDbContext restaurantDbContext, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public void Edit (int id, EditRestaurantDto editRestaurantDto)
        {
            var restaurant = _restaurantDbContext.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException(Messages.RestaurantNotFound);
            }

            var result = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new AccessRequirement(ActionOperation.Update)).Result;

            if (!result.Succeeded)
            {
                throw new ForbidException(Messages.Forbidden);
            }

            restaurant = _mapper.Map<EditRestaurantDto,Restaurant>(editRestaurantDto,restaurant);

            _restaurantDbContext.Update(restaurant);
            _restaurantDbContext.SaveChanges();

        }

        public int CreateRestaurant(CreateRestaurantDto restaurantDto)
        {
            var newRestaurant = _mapper.Map<Restaurant>(restaurantDto);

            if (_userContextService.UserId != null)
            {
                newRestaurant.UserId = (int)_userContextService.UserId;
                _restaurantDbContext.Restaurants.Add(newRestaurant);
                _restaurantDbContext.SaveChanges();
                return newRestaurant.Id;
            }
            else
            {
                throw new ForbidException(Messages.Forbidden);
            }
        }

        public List<RestaurantDto> GetAllRestaurants()
        {
            var restaurants = _restaurantDbContext.Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList();

            return _mapper.Map<List<RestaurantDto>>(restaurants);
        }

        public RestaurantDto GetRestaurantById(int id)
        {
            var restaurant = _restaurantDbContext.Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException(Messages.RestaurantNotFound);
            }

            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public void Delete(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action invoked");

            var restaurantToDelete = _restaurantDbContext.Restaurants.FirstOrDefault(r => r.Id == id);
            
            if (restaurantToDelete is null)
            {
                throw new NotFoundException(Messages.RestaurantNotFound);
            }

            var result = _authorizationService.AuthorizeAsync(_userContextService.User, restaurantToDelete, new AccessRequirement(ActionOperation.Delete)).Result;

            if (!result.Succeeded)
            {
                throw new ForbidException(Messages.Forbidden);
            }
            
            _restaurantDbContext.Restaurants.Remove(restaurantToDelete);
            _restaurantDbContext.SaveChanges();

        }

    }
}
