using Boilerplate.Contracts.DTOs;
using System.Data;

namespace Boilerplate.Contracts.Repositories;

public interface IAuthRepository : IGenericRepository<UserDto>
{
    Task<UserDto> GetAspNetUserAsync(UserDto userInfo);
    Task<UserDto> GetAspNetUserByPasswordAsync(UserDto userInfo);
    Task<(UserControlDto menuPermissions, int[] menuPermitted)> GetUserControlsInfo(string id);
    Task<IList<MenuDto>> GetAllPermittedMenu(string userId);
    Task<(IList<PagewiseActionDto> buttonPermissions, IEnumerable<dynamic> permittedButtons)> GetButtonPermissins(string userId);
    Task<DataTable> GetAllMenuByUserId(string userId);
    Task<DataTable> ButtonAction(string userId, int menuID);
}
