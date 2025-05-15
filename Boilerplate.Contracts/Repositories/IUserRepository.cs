using Boilerplate.Contracts.DTOs;
using System.Data;

namespace Boilerplate.Contracts.Repositories;
public interface IUserRepository
{
    Task<DataSet> GetAllUser(DataTableParams dataTableParams);
    Task<bool> ActiveInactive(string userId, bool isActive);
    Task<bool> ResetPassword(string passwordhash, string userId);
    Task<bool> CheckPassword(string passwordhash);
    Task<DataSet> GetUserBasicData();
    Task<bool> SaveUser(AspNetUserDto asp, UserControlDto tbluser, List<PagewiseActionDto> pagewiseActions);
    Task<bool> DeleteUser(string userId);
    Task<bool> EditUser(string menu, UserCreate model, List<PagewiseActionDto> pagewiseActions);
    Task<DataSet> GetUserEditData(string userId);
    Task<DataTable> GetUserAutoId();
    Task<DataTable> GetButtonActionByActionPermission(string actionPermission);
}
