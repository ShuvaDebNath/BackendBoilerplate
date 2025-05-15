using Boilerplate.Contracts;
using Boilerplate.Contracts.DTOs;
using Boilerplate.Contracts.Repositories;
using Boilerplate.Contracts.Services;
using Boilerplate.Entities.DBModels;
using Boilerplate.Entities.Helpers;
using System.Data;

namespace Boilerplate.Service.Services
{
    class UserService : IUserService
    {
        private readonly IUserRepository _createUserRepository;

        public UserService(IUserRepository createUserRepository)
        {
            _createUserRepository = createUserRepository;
        }

        public async Task<(List<User> data, int totalCount, int filterCount)> GetAllUser(DataTableParams dataTableParams)
        {
            var userList = new List<User>();

            var users = await _createUserRepository.GetAllUser(dataTableParams);

            if (users.Tables[0].Rows.Count > 0)
            {
                for (var index = 0; index < users.Tables[0].Rows.Count; index++)
                {
                    var user = new User()
                    {
                        RowIndex = Convert.ToInt32(users.Tables[0].Rows[index]["RowIndex"].ToString()),
                        Id = users.Tables[0].Rows[index]["Id"].ToString(),
                        MenuId = users.Tables[0].Rows[index]["MenuId"].ToString(),
                        UserName = users.Tables[0].Rows[index]["UserName"].ToString(),
                        UserId = users.Tables[0].Rows[index]["UserId"].ToString(),
                        UserTypeName = users.Tables[0].Rows[index]["UserTypeName"].ToString(),
                        isActive = Convert.ToBoolean(users.Tables[0].Rows[index]["isActive"].ToString()),
                        DashboardPreview = Convert.ToBoolean(users.Tables[0].Rows[index]["DashboardPreview"].ToString())
                    };

                    userList.Add(user);
                }

                var totalCount = users.Tables[0].Rows[0]["totalCount"].ToString();
                var filterCount = users.Tables[0].Rows[0]["filterCount"].ToString();

                return (userList, Convert.ToInt32(totalCount), Convert.ToInt32(filterCount));
            }
            else
            {
                return (userList, 0, 0);
            }
        }

        public async Task<bool> ActiveInactive(string userId, bool isActive)
        {
            var result = await _createUserRepository.ActiveInactive(userId, isActive);
            return result;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var result = await _createUserRepository.DeleteUser(userId);
            return result;
        }

        public async Task<bool> ResetPassword(string passwordhash, string userId)
        {
            var result = await _createUserRepository.ResetPassword(passwordhash, userId);
            return result;
        }

        public async Task<bool> SaveUser(UserMenuAssign data, string currentUserEmail)
        {
            UserCreate model = data.Data;
            List<MenuPerssion> details = data.DetailsData;

            #region menu configure code
            string menu = "";
            foreach (MenuPerssion d in details)
            {
                if (d.selected == true)
                {
                    menu += d.MenuId + ",";
                }
            }
            menu = menu.Substring(0, (menu.Length - 1));
            #endregion

            int AutoId = 0;

            var dt = await _createUserRepository.GetUserAutoId();
            AutoId = Convert.ToInt32(dt.Rows[0]["AutoId"].ToString());

            string AutoIdGen = ""; string Email = "";
            string fullname = "";
            AutoIdGen = model.FirstName + "-0" + AutoId;
            Email = AutoIdGen + "@gmail.com";
            fullname = model.FirstName + " " + model.LastName;

            var UserId = Guid.NewGuid().ToString();

            model.Password = "Maliha00";


            Random rnd = new Random();
            int pin = rnd.Next(1000, 9999);


            AspNetUserDto asp = new AspNetUserDto
            {
                Id = UserId,
                Email = Email,
                EmailConfirmed = true,
                PasswordHash = HelperExtention.Hash(model.Password),
                UserName = AutoIdGen,
                PasswordPin = pin.ToString("0000"),
            };

            UserControlDto tbluser = new UserControlDto
            {
                UserId = Guid.NewGuid().ToString(),
                Id = asp.Id,
                FullName = fullname,
                UserTypeId = model.UserTypeId,
                MenuId = menu,
                MakeBy = currentUserEmail,
                MakeDate = DateTime.Now,
                isActive = true,
                DashboardPreview = model.DashboardPreview,

            };

            var pagewiseAction = new List<PagewiseActionDto>();

            foreach (MenuPerssion d in details)
            {
                string menuPermission = "";
                PagewiseActionDto obj = new PagewiseActionDto();

                if (d.selected == true && d.ysnParent == false)
                {
                    obj.ActionID = Guid.NewGuid().ToString();
                    obj.UserId = tbluser.UserId;
                    obj.MenuId = d.MenuId;

                    if (d.insert == true)
                    {
                        menuPermission += 1 + ",";
                    }

                    if (d.update == true)
                    {
                        menuPermission += 2 + ",";
                    }

                    if (d.delete == true)
                    {
                        menuPermission += 3 + ",";
                    }

                    if (d.approve == true)
                    {
                        menuPermission += 4 + ",";
                    }

                    if (menuPermission == null || menuPermission == "")
                        menuPermission = "";
                    else
                        menuPermission = menuPermission.Substring(0, (menuPermission.Length - 1));

                    obj.ActionPermission = menuPermission;
                    pagewiseAction.Add(obj);
                }
            }

            var result = await _createUserRepository.SaveUser(asp, tbluser, pagewiseAction);
            return result;
        }

        public async Task<bool> EditUser(UserMenuAssign data, string currentUserEmail)
        {
            UserCreate model = data.Data;
            List<MenuPerssion> details = data.DetailsData;

            #region menu configure code
            string menu = "";
            foreach (MenuPerssion d in details)
            {
                if (d.selected == true)
                {
                    menu += d.MenuId + ",";
                }
            }
            menu = menu.Substring(0, (menu.Length - 1));
            #endregion

            var pagewiseAction = new List<PagewiseActionDto>();

            foreach (MenuPerssion d in details)
            {
                string menuPermission = "";
                PagewiseActionDto obj = new PagewiseActionDto();

                if (d.selected == true && d.ysnParent == false)
                {
                    obj.ActionID = Guid.NewGuid().ToString();
                    obj.UserId = model.UserId;
                    obj.MenuId = d.MenuId;

                    if (d.insert == true)
                    {
                        menuPermission += 1 + ",";
                    }

                    if (d.update == true)
                    {
                        menuPermission += 2 + ",";
                    }

                    if (d.delete == true)
                    {
                        menuPermission += 3 + ",";
                    }

                    if (d.approve == true)
                    {
                        menuPermission += 4 + ",";
                    }

                    if (menuPermission == null || menuPermission == "")
                        menuPermission = "";
                    else
                        menuPermission = menuPermission.Substring(0, (menuPermission.Length - 1));

                    obj.ActionPermission = menuPermission;
                    pagewiseAction.Add(obj);
                }
            }

            var result = await _createUserRepository.EditUser(menu, model, pagewiseAction);
            return result;
        }

        public async Task<DataSet> GetUserBasicData()
        {
            DataSet ds = await _createUserRepository.GetUserBasicData();

            ds.Tables[0].TableName = "usertype";
            ds.Tables[1].TableName = "userRole";
            ds.Tables[2].TableName = "allmenu";

            return ds;
        }

        public async Task<(DataTable userName, List<MenuPerssion> selectedMenu)> GetUserEditData(string userId)
        {
            var ds = await _createUserRepository.GetUserEditData(userId);

            List<Menus> menus = new List<Menus>();

            if (ds.Tables[1].Rows.Count > 0)
            {
                for (var index = 0; index < ds.Tables[1].Rows.Count; index++)
                {
                    var menu = new Menus()
                    {
                        MenuId = Convert.ToInt32(ds.Tables[1].Rows[index]["MenuId"].ToString()),
                        SubMenuName = ds.Tables[1].Rows[index]["SubMenuName"].ToString()
                    };

                    menus.Add(menu);
                }
            }

            List<MenuPerssion> selectedMenu = new List<MenuPerssion>();

            if (ds.Tables[2].Rows.Count > 0)
            {
                for (var index = 0; index < ds.Tables[2].Rows.Count; index++)
                {
                    MenuPerssion aMenu = new MenuPerssion();

                    aMenu.MenuId = Convert.ToInt32(ds.Tables[2].Rows[index]["MenuId"].ToString());
                    aMenu.SubMenuName = ds.Tables[2].Rows[index]["SubMenuName"].ToString();
                    aMenu.selected = false;
                    aMenu.ysnParent = Convert.ToBoolean(ds.Tables[2].Rows[index]["ysnParent"].ToString());
                    aMenu.OrderBy = Convert.ToInt32(ds.Tables[2].Rows[index]["OrderBy"].ToString());

                    for (int j = 0; j < menus.Count; j++)
                    {
                        if (Convert.ToInt32(ds.Tables[2].Rows[index]["MenuId"].ToString()) == menus[j].MenuId)
                        {
                            aMenu.selected = true;
                            for (int k = 0; k < ds.Tables[3].Rows.Count; k++)
                            {
                                if (menus[j].MenuId == Convert.ToInt32(ds.Tables[3].Rows[k]["MenuId"].ToString()))
                                {
                                    var ActionPermission = ds.Tables[3].Rows[k]["ActionPermission"].ToString();
                                    if (ActionPermission != "")
                                    {
                                        var actionList = await _createUserRepository.GetButtonActionByActionPermission(ActionPermission);

                                        if (actionList.Rows.Count > 0)
                                        {
                                            ButtonAction aBUtton = new ButtonAction();
                                            for (int l = 0; l < actionList.Rows.Count; l++)
                                            {
                                                aBUtton.Id = Convert.ToInt32(actionList.Rows[l]["Id"].ToString());

                                                if (aBUtton.Id == 1)
                                                {
                                                    aMenu.insert = true;
                                                }
                                                if (aBUtton.Id == 2)
                                                {
                                                    aMenu.update = true;
                                                }
                                                if (aBUtton.Id == 3)
                                                {
                                                    aMenu.delete = true;
                                                }
                                                if (aBUtton.Id == 4)
                                                {
                                                    aMenu.approve = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                    selectedMenu.Add(aMenu);
                }
            }
            ds.Tables[0].TableName = "userName";
            return (ds.Tables[0], selectedMenu);
        }

        public Task<bool> CheckPassword(string passwordhash)
        {
          return _createUserRepository.CheckPassword(passwordhash);

        }
    }
}
