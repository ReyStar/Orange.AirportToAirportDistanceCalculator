using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Routes.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UserAccessAttribute: ActionFilterAttribute
    {
        //https://stackoverflow.com/questions/38977088/asp-net-core-web-api-authentication
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext?.User;
            if (user != null)
            {
                if (user.IsInRole("Admin"))
                {
                    return;
                }

                if (Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var authUserId))
                {
                    if (context.ActionArguments.TryGetValue("userId", out var userIdValue))
                    {
                        if (userIdValue is Guid userId)
                        {
                             if (userId == authUserId)
                             {
                                 return;
                             }
                        }
                    }
                }
            }
            
            context.Result = new ObjectResult(context.ModelState)
                             {
                                 Value = "access denied",
                                 StatusCode = StatusCodes.Status403Forbidden
                             };
        }
    }
}
