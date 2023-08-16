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
        Task<(tblUserControl menuPermissions, int[] menuPermitted)> GetUserControlsInfo(string id);
        Task<IList<Menu>> GetAllPermittedMenu(string userId);
        Task<(IList<tblPagewiseAction> buttonPermissions, IEnumerable<dynamic> permittedButtons)> GetButtonPermissins(string userId);
        Task<IList<tblMenu>> GetAllMenuByUserId(string userId);
        Task<IList<tblButtonAction>> ButtonAction(string userId, int menuID);
    }

}
