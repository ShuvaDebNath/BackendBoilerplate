using Boilerplate.Entities.DBModels;
using Boilerplate.Entities.DTOs;
using Boilerplate.Entities.DTOs.UserCreate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Service.Interfaces
{
    public interface IAuthService
    {
        Task<UserInfo> GetAspNetUserAsync(UserInfo userInfo);
        Task<UserInfo> GetAspNetUserByPasswordAsync(UserInfo userInfo);
        Task<(UserControl menuPermissions, int[] menuPermitted)> GetUserControlsInfo(string id);
        Task<IList<Menu>> GetAllPermittedMenu(string userId);
        Task<(IList<PagewiseAction> buttonPermissions, IEnumerable<dynamic> permittedButtons)> GetButtonPermissins(string userId);
        Task<IList<Menus>> GetAllMenuByUserId(string userId);
        Task<IList<ButtonAction>> ButtonAction(string userId, int menuID);
    }

}
