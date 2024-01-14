using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Boilerplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private string UserName => HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserName").Value;
        private string UserId => HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
        private string Email => HttpContext.User.FindFirstValue(ClaimTypes.Email);
        private string UserRole => HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserRole").Value;
        private string UserType => HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserType").Value;

        protected string AuthUserName
        {
            get { return UserName; }
        }
        protected string AuthEmail
        {
            get { return Email; }
        }
        protected string AuthUserId
        {
            get { return UserId; }
        }
        protected string AuthUserRole
        {
            get { return UserRole; }
        }
        protected string AuthUserType
        {
            get { return UserType; }
        }
    }
}