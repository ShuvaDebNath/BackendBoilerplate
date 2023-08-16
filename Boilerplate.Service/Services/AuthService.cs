using Boilerplate.Entities.DBModels;
using Boilerplate.Entities.DTOs;
using Boilerplate.Entities.DTOs.UserCreate;
using Boilerplate.Entities.Helpers;
using Boilerplate.Repository.Interfaces;
using Boilerplate.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
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
        public async Task<(tblUserControl menuPermissions, int[] menuPermitted)> GetUserControlsInfo(string id)
        {
            var (menuPermissions, menuPermitted) = await _authRepository.GetUserControlsInfo(id);

            return (menuPermissions, menuPermitted);
        }

        public async Task<IList<Menu>> GetAllPermittedMenu(string userId)
        {
            try
            {
                var permittedMenus = await _authRepository.GetAllPermittedMenu(userId);

                return permittedMenus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<tblButtonAction>> ButtonAction(string userId, int menuID)
        {
            try
            {
                var allMenuList = await _authRepository.ButtonAction(userId,menuID);

                List<tblButtonAction> btns = new List<tblButtonAction>();

                if (allMenuList.Rows.Count > 0)
                {
                    for (var index = 0; index < allMenuList.Rows.Count; index++)
                    {
                        tblButtonAction btnMenu = new tblButtonAction();
                        btnMenu.Id = int.Parse(allMenuList.Rows[index]["Id"].ToString());
                        btns.Add(btnMenu);
                    }
                }

                return btns;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<IList<tblMenu>> GetAllMenuByUserId(string userId)
        {
            try
            {
                var permittedMenus = await _authRepository.GetAllMenuByUserId(userId);

                List<tblMenu> menus = new List<tblMenu>();

                if (permittedMenus.Rows.Count > 0)
                {
                    for (var index = 0; index < permittedMenus.Rows.Count; index++)
                    {
                        tblMenu menu = new tblMenu();
                        menu.UiLink = permittedMenus.Rows[index]["UiLink"].ToString();
                        menu.MenuId = int.Parse(permittedMenus.Rows[index]["MenuId"].ToString());
                        menus.Add(menu);
                    }
                }

                return menus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<(IList<tblPagewiseAction> buttonPermissions, IEnumerable<dynamic> permittedButtons)> GetButtonPermissins(string userId)
        {
            try
            {
                var (buttonPermissions, permittedButtons) = await _authRepository.GetButtonPermissins(userId);

                return (buttonPermissions, permittedButtons);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
