using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Authorization
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinAge { get; set; }
        public MinimumAgeRequirement(int minAge)
        {
            MinAge = minAge;
        }
    }
}
