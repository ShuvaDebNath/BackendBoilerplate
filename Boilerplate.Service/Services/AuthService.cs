using Boilerplate.Entities.DBModels;
using Boilerplate.Entities.DTOs;
using Boilerplate.Entities.DTOs.UserCreate;
using Boilerplate.Entities.Helpers;
using Boilerplate.Repository.Interfaces;
using Boilerplate.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<GetDataService> _logger;

        public AuthService(IAuthRepository authRepository, ILogger<GetDataService> logger)
        {
            _authRepository = authRepository;
            _logger = logger;
        }

        public async Task<UserInfo> GetAspNetUserAsync(UserInfo userInfo)
        {
            var passwordHash = HelperExtention.Hash(userInfo.Password);
            userInfo.Password = passwordHash;
            var result = await _authRepository.GetAspNetUserAsync(userInfo);

            return result;
        }
        public async Task<UserInfo> GetAspNetUserByPasswordAsync(UserInfo userInfo)
        {
            var passwordHash = HelperExtention.Hash(userInfo.Password);
            userInfo.Password = passwordHash;
            var result = await _authRepository.GetAspNetUserByPasswordAsync(userInfo);

            return result;
        }
        public async Task<(UserControl menuPermissions, int[] menuPermitted)> GetUserControlsInfo(string id)
        {
            var (menuPermissions, menuPermitted) = await _authRepository.GetUserControlsInfo(id);

            return (menuPermissions, menuPermitted);
        }

        public async Task<IList<Menu>> GetAllPermittedMenu(string userId)
        {
            try
            {
                var permittedMenus = await _authRepository.GetAllPermittedMenu(userId);

                if (permittedMenus.Count > 0)
                {
                    _logger.LogInformation($"Data Found!");
                    return permittedMenus;
                }
                _logger.LogInformation($"Data Not Found!");
                return (IList<Menu>)Enumerable.Empty<Menu>();
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Source: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<ButtonAction>> ButtonAction(string userId, int menuID)
        {
            try
            {
                var allMenuList = await _authRepository.ButtonAction(userId,menuID);

                List<ButtonAction> btns = new List<ButtonAction>();

                if (allMenuList.Rows.Count > 0)
                {
                    for (var index = 0; index < allMenuList.Rows.Count; index++)
                    {
                        ButtonAction btnMenu = new ButtonAction();
                        btnMenu.Id = int.Parse(allMenuList.Rows[index]["Id"].ToString());
                        btns.Add(btnMenu);
                    }
                    _logger.LogInformation($"Data Found!");
                    return btns;
                }
                _logger.LogInformation($"Data Not Found!");
                return (IList<ButtonAction>)Enumerable.Empty<ButtonAction>();
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Source: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<IList<Menus>> GetAllMenuByUserId(string userId)
        {
            try
            {
                var permittedMenus = await _authRepository.GetAllMenuByUserId(userId);

                List<Menus> menus = new List<Menus>();

                if (permittedMenus.Rows.Count > 0)
                {
                    for (var index = 0; index < permittedMenus.Rows.Count; index++)
                    {
                        Menus menu = new Menus();
                        menu.UiLink = permittedMenus.Rows[index]["UiLink"].ToString();
                        menu.MenuId = int.Parse(permittedMenus.Rows[index]["MenuId"].ToString());
                        menus.Add(menu);
                    }
                    _logger.LogInformation($"Data Found!");
                    return menus;
                }
                _logger.LogInformation($"Data Not Found!");
                return (IList<Menus>)Enumerable.Empty<Menus>();
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Source: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw new Exception(ex.Message);
            }
        }

        public async Task<(IList<PagewiseAction> buttonPermissions, IEnumerable<dynamic> permittedButtons)> GetButtonPermissins(string userId)
        {
            try
            {
                var (buttonPermissions, permittedButtons) = await _authRepository.GetButtonPermissins(userId);

                return (buttonPermissions, permittedButtons);
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Source: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw new Exception(ex.Message);
            }
        }
    }
}
