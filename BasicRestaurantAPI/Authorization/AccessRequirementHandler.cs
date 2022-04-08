using BasicRestaurantAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Authorization
{
    public class AccessRequirementHandler : AuthorizationHandler<AccessRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRequirement requirement, Restaurant resource)
        {
            if (requirement.ActionOperation == ActionOperation.Read || requirement.ActionOperation == ActionOperation.Create)
            {
                context.Succeed(requirement);
            }
            
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if(userId == resource.UserId.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
