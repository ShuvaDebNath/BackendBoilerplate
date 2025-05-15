using Boilerplate.Contracts.DTOs;

namespace Boilerplate.Contracts.Services;

public interface IAuthService
{
    Task<UserDto> GetAspNetUserAsync(UserDto userInfo);
    Task<UserDto> GetAspNetUserByPasswordAsync(UserDto userInfo);
    Task<(UserControlDto menuPermissions, int[] menuPermitted)> GetUserControlsInfo(string id);
    Task<IList<MenuDto>> GetAllPermittedMenu(string userId);
    Task<(IList<PagewiseActionDto> buttonPermissions, IEnumerable<dynamic> permittedButtons)> GetButtonPermissins(string userId);
    Task<IList<MenusDto>> GetAllMenuByUserId(string userId);
    Task<IList<ButtonAction>> ButtonAction(string userId, int menuID);
}
