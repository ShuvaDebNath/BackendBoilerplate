
using Boilerplate.Entities.DBModels;
using Boilerplate.Entities.DTOs;
using Boilerplate.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Boilerplate.Repository.Interfaces
{
    public interface IAuthRepository : IGenericRepository<UserInfo>
    {
        Task<UserInfo> GetAspNetUserAsync(UserInfo userInfo);
        Task<UserInfo> GetAspNetUserByPasswordAsync(UserInfo userInfo);
        Task<(tblUserControl menuPermissions, int[] menuPermitted)> GetUserControlsInfo(string id);
        Task<IList<Menu>> GetAllPermittedMenu(string userId);
        Task<(IList<tblPagewiseAction> buttonPermissions, IEnumerable<dynamic> permittedButtons)> GetButtonPermissins(string userId);
        Task<DataTable> GetAllMenuByUserId(string userId);
        Task<DataTable> ButtonAction(string userId, int menuID);
    }
}
