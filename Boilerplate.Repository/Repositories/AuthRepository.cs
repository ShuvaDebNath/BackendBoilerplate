using Boilerplate.Entities.DBModels;
using Boilerplate.Entities.DTOs;
using Boilerplate.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Boilerplate.Repository.Repositories
{
    
    public class AuthRepository : GenericRepository<UserInfo>, IAuthRepository
    {
        public AuthRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<UserInfo> GetAspNetUserAsync(UserInfo userInfo)
        {
            const string query = "SELECT Id,Email,EmailConfirmed,UserName,PasswordPin FROM AspNetUsers where UserName = @UserName and PasswordHash = @PasswordHash";

            var result = await GetAllAsync(query, new { userInfo.UserName, PasswordHash = userInfo.Password, PasswordPin = userInfo.PasswordPin });
            return result.FirstOrDefault();
        }
        public async Task<UserInfo> GetAspNetUserByPasswordAsync(UserInfo userInfo)
        {
            const string query = "SELECT Id,Email,EmailConfirmed,UserName FROM AspNetUsers where PasswordHash = @PasswordHash";

            var result = await GetAllAsync(query, new { userInfo.UserName, PasswordHash = userInfo.Password });
            return result.FirstOrDefault();
        }

        public async Task<(UserControl menuPermissions, int[] menuPermitted)> GetUserControlsInfo(string userId)
        {
            const string query = "select UserId Id,FullName,Id UserId,UserTypeId,MenuId,UserRoleID from tblUserControl where Id = @UserId";
            DataTable dt = await GetDataInDataTableAsync(query, new { UserId = userId });
            UserControl menuPermissions = new UserControl();

            if (dt.Rows.Count > 0)
            {
                menuPermissions = new UserControl()
                {
                    MenuId = dt.Rows[0]["MenuId"].ToString(),
                    UserId = dt.Rows[0]["UserId"].ToString(),
                    UserTypeId = dt.Rows[0]["UserTypeId"].ToString(),
                    FullName = dt.Rows[0]["FullName"].ToString(),
                    Id = dt.Rows[0]["Id"].ToString(),
                    UserRoleID = dt.Rows[0]["UserRoleID"].ToString()
                };

                var menus = dt.Rows[0]["MenuId"].ToString().Split(',');
                int[] menuPermitted = menus.Select(int.Parse).ToArray();

                return (menuPermissions, menuPermitted);
            }

            return (menuPermissions, null);
        }

        public async Task<IList<Menu>> GetAllPermittedMenu(string userId)
        {
            const string query = @"select MenuId,rtrim(ltrim(MenuName)) MenuName,SubMenuName,UiLink,isActive,ysnParent,OrderBy,MenuLogo 
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

            var permittedMenus = new List<Menu>();

            foreach (DataRow parentMenu in parentMenus.Rows)
            {
                DataRow[] childs = childMenus.Select("MenuName='" + parentMenu.ItemArray[1].ToString() + "'");
                var tempChilds = new List<Menus>();

                if (childs.Any())
                {
                    foreach (DataRow child in childs)
                    {
                        Menus aChild = new Menus()
                        {
                            MenuId = int.Parse(child["MenuId"].ToString()),
                            MenuName = child["MenuName"].ToString(),
                            SubMenuName = child["SubMenuName"].ToString(),
                            UiLink = child["UiLink"].ToString(),
                            isActive = bool.Parse(child["isActive"].ToString()),
                            ysnParent = bool.Parse(child["ysnParent"].ToString()),
                            OrderBy = int.Parse(child["OrderBy"].ToString()),
                            MenuLogo = child["MenuLogo"].ToString(),
                        };
                        tempChilds.Add(aChild);
                    }
                }


                var parent = new Menu(parentMenu["MenuName"].ToString(), parentMenu["MenuLogo"].ToString(), bool.Parse(parentMenu["isActive"].ToString()), parentMenu["UiLink"].ToString(), tempChilds);
                permittedMenus.Add(parent);
            }
            return permittedMenus.ToList();
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
        
        public async Task<DataTable> ButtonAction(string userId,int menuID)
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

        public async Task<(IList<PagewiseAction> buttonPermissions, IEnumerable<dynamic> permittedButtons)> GetButtonPermissins(string userId)
        {
            const string query = "select ActionID,UserId,MenuId,ActionPermission from tblPagewiseAction where UserId = @UserId";

            DataTable dtButtonPermissions = await GetDataInDataTableAsync(query, new { UserId = userId });

            if (dtButtonPermissions.Rows.Count > 0)
            {
                List<PagewiseAction> buttonPermissions = new();
                buttonPermissions.AddRange(
                    from DataRow dr in dtButtonPermissions.Rows
                    select new PagewiseAction()
                    {
                        ActionID = dr["ActionID"].ToString(),
                        ActionPermission = dr["ActionPermission"].ToString(),
                        MenuId = int.Parse(dr["MenuId"].ToString()),
                        UserId = dr["UserId"].ToString(),
                    });

                List<PagewiseAction> permittedButton = new List<PagewiseAction>();

                permittedButton.AddRange(
                    from DataRow dr in dtButtonPermissions.Rows
                    select new PagewiseAction()
                    {
                        MenuId = int.Parse(dr["MenuId"].ToString()),
                        ActionPermissionArrya = dr["ActionPermission"].ToString().Split(",")
                    });

                return (buttonPermissions.ToList(), permittedButton);
            }
            else
            {
                return (null, null);
            }
        }
    }
}
