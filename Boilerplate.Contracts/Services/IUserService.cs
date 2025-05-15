using System.Data;

namespace Boilerplate.Contracts.Services;

public interface IUserService
{
    Task<(List<User> data, int totalCount, int filterCount)> GetAllUser(DataTableParams dataTableParams);
    Task<bool> ActiveInactive(string userId, bool isActive);
    Task<bool> ResetPassword(string passwordhash, string userId);
    Task<DataSet> GetUserBasicData();
    Task<bool> SaveUser(UserMenuAssign data,string currentUserEmail);
    Task<bool> DeleteUser(string userId);
    Task<bool> EditUser(UserMenuAssign data, string currentUserEmail);
    Task<(DataTable userName, List<MenuPerssion> selectedMenu)> GetUserEditData(string userId);
    Task<bool> CheckPassword(string passwordhash);
}
