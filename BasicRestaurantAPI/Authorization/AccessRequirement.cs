using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicRestaurantAPI.Authorization
{
    public enum ActionOperation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class AccessRequirement : IAuthorizationRequirement
    {
        public ActionOperation ActionOperation { get; }
        
        public AccessRequirement(ActionOperation resourceOperation)
        {
            ActionOperation = resourceOperation;
        }
           
    }
}
