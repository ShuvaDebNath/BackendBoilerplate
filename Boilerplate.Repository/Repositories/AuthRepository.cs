using Boilerplate.Contracts;
using Boilerplate.Contracts.DTOs;
using Boilerplate.Contracts.Repositories;
using Boilerplate.Entities.DBModels;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Boilerplate.Repository.Repositories;

public class AuthRepository : GenericRepository<UserDto>, IAuthRepository
{
    public AuthRepository(IConfiguration configuration) : base(configuration)
    {

    }
    public async Task<DataTable> GetAllMenuByUserId(string userId)
    {
        const string query = @"
            declare @MenuId nvarchar(MAX) = ''
            select @MenuId = uc.MenuId
            from tblUserControl uc
            join AspNetUsers au on uc.Id = au.Id
            join tblUserType ut on ut.UserTypeId = uc.UserTypeId
            where uc.Id = @UserId
            select  MenuId,MenuName,UiLink, SubMenuName from tblMenu where MenuId in ( select value from string_split(@MenuId,',') ) and ysnParent = 0";

        DataTable dtMenus = await GetDataInDataTableAsync(query, new { UserId = userId });

        return dtMenus;
    }

    public async Task<DataTable> ButtonAction(string userId, int menuID)
    {
        const string query = @"
            declare @ActionPermission nvarchar(MAX) = '',
            @UserId varchar(128),@MenuId int = 2

            select @UserId = UserId
            from tblUserControl
            where Id = @Id 

            select @ActionPermission = ActionPermission
            from tblPagewiseAction
            where UserId = @UserId and MenuId = @MenuId

            select Id,ActionName from tblButtonAction where Id IN (select value from string_split(@ActionPermission,','))";

        DataTable dtMenus = await GetDataInDataTableAsync(query, new { Id = userId });

        return dtMenus;
    }

    public async Task<UserDto> GetAspNetUserAsync(UserDto userInfo)
    {
        const string query = "SELECT Id,Email,EmailConfirmed,UserName,PasswordPin FROM AspNetUsers where UserName = @UserName and PasswordHash = @PasswordHash";

        var result = await GetAllAsync(query, new { userInfo.UserName, PasswordHash = userInfo.Password, PasswordPin = userInfo.PasswordPin });
        return result.FirstOrDefault();
    }

    public async Task<UserDto> GetAspNetUserByPasswordAsync(UserDto userInfo)
    {
        const string query = "SELECT Id,Email,EmailConfirmed,UserName FROM AspNetUsers where PasswordHash = @PasswordHash";

        var result = await GetAllAsync(query, new { userInfo.UserName, PasswordHash = userInfo.Password });
        return result.FirstOrDefault();
    }

    public async Task<(UserControlDto menuPermissions, int[] menuPermitted)> GetUserControlsInfo(string id)
    {
        const string query = "SELECT UserId, FullName, UserTypeId, MenuId, UserRoleID FROM tblUserControl WHERE Id = @UserId";
        DataTable dt = await GetDataInDataTableAsync(query, new { UserId = id });
        UserControlDto menuPermissions = new UserControlDto();

        if (dt.Rows.Count > 0)
        {
            menuPermissions = new UserControlDto()
            {
                MenuId = dt.Rows[0]["MenuId"].ToString(),
                UserId = dt.Rows[0]["UserId"].ToString(),
                UserTypeId = dt.Rows[0]["UserTypeId"].ToString(),
                FullName = dt.Rows[0]["FullName"].ToString(),
                Id = dt.Rows[0]["UserId"].ToString(),
                UserRoleID = dt.Rows[0]["UserRoleID"].ToString()
            };

            var menus = dt.Rows[0]["MenuId"].ToString().Split(',');
            int[] menuPermitted = menus.Select(int.Parse).ToArray();

            return (menuPermissions, menuPermitted);
        }

        return (menuPermissions, null);
    }


    public async Task<IList<MenuDto>> GetAllPermittedMenu(string userId)
    {
        const string query = @"select MenuId, rtrim(ltrim(MenuName)) MenuName, SubMenuName, UiLink, isActive, ysnParent, OrderBy, MenuLogo 
            from tblMenu menu 
            where MenuId in (select value from STRING_SPLIT((select MenuId from tblUserControl where Id = @UserId),',')) 
            order by OrderBy";

        DataTable dtMenus = await GetDataInDataTableAsync(query, new { UserId = userId });

        if (dtMenus.Rows.Count <= 0)
        {
            throw new Exception("Authentication Fail.");
        }

        DataTable parentMenus = dtMenus.Select("ysnParent=1").CopyToDataTable();
        DataTable childMenus = dtMenus.Select("ysnParent=0").CopyToDataTable();

        var permittedMenus = new List<MenuDto>();

        foreach (DataRow parentMenu in parentMenus.Rows)
        {
            DataRow[] childs = childMenus.Select("MenuName='" + parentMenu["MenuName"].ToString() + "'");
            var tempChilds = new List<MenusDto>();

            if (childs.Any())
            {
                foreach (DataRow child in childs)
                {
                    var aChild = new MenusDto()
                    {
                        MenuId = int.Parse(child["MenuId"].ToString()),
                        MenuName = child["MenuName"].ToString(),
                        SubMenuName = child["SubMenuName"].ToString(),
                        UiLink = child["UiLink"].ToString(),
                        isActive = child["isActive"] == DBNull.Value ? (bool?)null : bool.Parse(child["isActive"].ToString()),
                        ysnParent = child["ysnParent"] == DBNull.Value ? (bool?)null : bool.Parse(child["ysnParent"].ToString()),
                        OrderBy = child["OrderBy"] == DBNull.Value ? (int?)null : int.Parse(child["OrderBy"].ToString()),
                        MenuLogo = child["MenuLogo"].ToString()
                    };
                    tempChilds.Add(aChild);
                }
            }

            var parent = new MenuDto(
                parentMenu["MenuName"].ToString(),
                parentMenu["MenuLogo"].ToString(),
                bool.Parse(parentMenu["isActive"].ToString()),
                parentMenu["UiLink"].ToString(), tempChilds);

            permittedMenus.Add(parent);
        }

        return permittedMenus.ToList();
    }


    public async Task<(IList<PagewiseActionDto> buttonPermissions, IEnumerable<dynamic> permittedButtons)> GetButtonPermissins(string userId)
    {
        const string query = "SELECT ActionID, UserId, MenuId, ActionPermission FROM tblPagewiseAction WHERE UserId = @UserId";

        DataTable dtButtonPermissions = await GetDataInDataTableAsync(query, new { UserId = userId });

        if (dtButtonPermissions.Rows.Count > 0)
        {
            
            List<PagewiseActionDto> buttonPermissions = dtButtonPermissions
                .AsEnumerable()
                .Select(dr => new PagewiseActionDto
                {
                    ActionID = dr["ActionID"].ToString(),
                    ActionPermission = dr["ActionPermission"].ToString(),
                    MenuId = int.Parse(dr["MenuId"].ToString()),
                    UserId = dr["UserId"].ToString(),
                    ActionPermissionArrya = dr["ActionPermission"].ToString().Split(",")
                })
                .ToList();

           
            var permittedButtons = dtButtonPermissions
                .AsEnumerable()
                .Select(dr => new
                {
                    MenuId = int.Parse(dr["MenuId"].ToString()),
                    ActionPermissionArrya = dr["ActionPermission"].ToString().Split(",")
                })
                .ToList();

            return (buttonPermissions, permittedButtons);
        }
        else
        {
            return (null, null);
        }
    }

}
