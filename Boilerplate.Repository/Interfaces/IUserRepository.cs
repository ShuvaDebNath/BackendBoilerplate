using Boilerplate.Entities.DBModels;
using Boilerplate.Entities.DTOs;
using Boilerplate.Entities.DTOs.UserCreate;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Boilerplate.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<DataSet> GetAllUser(DataTableParams dataTableParams);
        Task<bool> ActiveInactive(string userId, bool isActive);
        Task<bool> ResetPassword(string passwordhash, string userId);
        Task<bool> CheckPassword(string passwordhash);
        Task<DataSet> GetUserBasicData();
        Task<bool> SaveUser(AspNetUser asp, tblUserControl tbluser, List<tblPagewiseAction> pagewiseActions);
        Task<bool> DeleteUser(string userId);
        Task<bool> EditUser(string menu, UserCreate model, List<tblPagewiseAction> pagewiseActions);
        Task<DataSet> GetUserEditData(string userId);
        Task<DataTable> GetUserAutoId();
        Task<DataTable> GetButtonActionByActionPermission(string actionPermission);
    }

}
