using Boilerplate.Contracts;
using Boilerplate.Contracts.Enum;
using Boilerplate.Contracts.Responses;
using Boilerplate.Contracts.Services;
using Boilerplate.Entities.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Boilerplate.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseApiController
    {
        private readonly IConfiguration _config;
        protected readonly IAuthService _authService;

        public AccountsController(IConfiguration config,
            IAuthService authService)
        {
            _config = config;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] UserDto userInfo)
        {
            var user = await _authService.GetAspNetUserAsync(userInfo);

            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                //var (menuPermissions, menuPermitted) = await _authService.GetUserControlsInfo(user.Id);

                //user.UserTypeId = menuPermissions.UserTypeId;
                var tokenString = GetToken(user);
                //var permittedMenu = await _authService.GetAllPermittedMenu(user.Id);
                //var (buttonPermissions, permittedButtons) = await _authService.GetButtonPermissins(menuPermissions.UserId);

                return Ok(new { IsAuthorized = true, TOKEN = tokenString , //PermittedMenus = permittedMenu, PermittedButtons = permittedButtons,
                    UserName = user.UserName, //FullName = menuPermissions.FullName, UserId = menuPermissions.UserId,
                    CompanyId = "bfd4ca78-3ec9-4798-8be9-5a9423917949",
                    user.PasswordPin });
            }
            else
            {
                return Ok(new { IsAuthorized = false, IsViewWalkthrough = false, Data = (string)null, Message = MessageTypes.InvalidUser, MessageType = MessageTypes.UnAuthorize });
            }
        }

        [AllowAnonymous]
        [HttpPost(nameof(LoginWithPassword))]
        public async Task<IActionResult> LoginWithPassword([FromBody] UserDto userInfo)
        {
            var user = await _authService.GetAspNetUserByPasswordAsync(userInfo);

            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                var (menuPermissions, menuPermitted) = await _authService.GetUserControlsInfo(user.Id);

                user.UserTypeId = menuPermissions.UserTypeId;
                var tokenString = GetToken(user);
                var permittedMenu = await _authService.GetAllPermittedMenu(user.Id);
                var (buttonPermissions, permittedButtons) = await _authService.GetButtonPermissins(menuPermissions.UserId);

                return Ok(new { IsAuthorized = true, TOKEN = tokenString, PermittedMenus = permittedMenu, PermittedButtons = permittedButtons, 
                    UserName = user.UserName, FullName = menuPermissions.FullName, 
                    UserId = menuPermissions.UserId, CompanyId = "bfd4ca78-3ec9-4798-8be9-5a9423917949",
                    UserType = user.UserTypeId
                });
            }
            else
            {
                return Ok(new { IsAuthorized = false, IsViewWalkthrough = false, Data = (string)null, Message = MessageTypes.InvalidUser, MessageType = MessageTypes.UnAuthorize });
            }
        }

        [HttpPost]
        [Route("GetMenuAndButton")]
        public async Task<IActionResult> GetMenuAndButton()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var (menuPermissions, menuPermitted) = await _authService.GetUserControlsInfo(AuthUserId);

                    var permittedMenu = await _authService.GetAllPermittedMenu(AuthUserId);

                    var (buttonPermissions, permittedButtons) = await _authService.GetButtonPermissins(menuPermissions.UserId);

                    return Ok(new { ok = true, PermittedMenus = permittedMenu, PermittedButtons = permittedButtons });
                }
                else
                {
                    return Ok(new { ok = false, msg = MessageTypes.InValidToken, innermsg = MessageTypes.NotFound });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { ok = false, msg = ex.Message, innermsg = ex.InnerException });
            }
        }

        #region AuthenticateCheck
        [HttpPost(nameof(AuthenticateCheck))]
        public async Task<IActionResult> AuthenticateCheck(string controller)
        {
            var menus = await _authService.GetAllMenuByUserId(AuthUserId);

            int flag = 0; int menuID = 0;
            for (int i = 0; i < menus.Count; i++)
            {
                var menuActionName = menus[i].UiLink;

                if (menuActionName != "" && menuActionName != null)
                {
                    var findControllerName = menuActionName;

                    if (controller == findControllerName)
                    {
                        flag = 1;
                        menuID = menus[i].MenuId;
                        break;
                    }
                }
            }

            return Ok(MessageType.DataFound(new { Flag = flag, MENUID = menuID }));
        }
        #endregion

        #region ButtonAction
        [HttpPost(nameof(ButtonAction))]
        public async Task<IActionResult> ButtonAction(int menuID)
        {
            var btns = await _authService.ButtonAction(AuthUserId,menuID);

            return Ok(MessageType.DataFound(new { Btns = btns }));
        }
        #endregion

        private string GetToken(UserDto userInfo)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecurityJwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(TokenVariableParams.UserId, userInfo.Id),
                new Claim(TokenVariableParams.UserName, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            JwtSecurityToken token = new JwtSecurityToken(_config["SecurityJwt:Issuer"], _config["SecurityJwt:Issuer"],
                                             claims,
                                             notBefore: DateTime.UtcNow,
                                             expires: DateTime.Now.AddHours(12),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}