using Boilerplate.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Boilerplate.API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (HasAllowAnonymous(context)) return; // if AllowAnonymous

            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString(); // get token from header
                                                                                              
            if (string.IsNullOrEmpty(authHeader) && !authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new CustomUnauthorizedResult(false, true, "Access token is empty.", "EmptyToken", null);
                return;
            }
           
            var currentUser = context.HttpContext.User; // get current user

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (!currentUser.Identity.IsAuthenticated)
            {
                context.Result = new CustomUnauthorizedResult(false, false, "UnAuthorization Access to User!", "UnAuthorization", null);
                return;
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return;
        }

        public static bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            return context.Filters.Any(item => item is IAllowAnonymousFilter);
        }

        public class CustomUnauthorizedResult : JsonResult
        {
            public CustomUnauthorizedResult(bool status, bool isAuthorized, string message, string messageType, object data)
                : base(new Messages { Status = status, IsAuthorized= isAuthorized, Message = message, Data = data , MessageType = messageType })
            {
                StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
            }
        }
    }
}