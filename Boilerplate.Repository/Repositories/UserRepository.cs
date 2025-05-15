using Boilerplate.Contracts;
using Boilerplate.Contracts.DTOs;
using Boilerplate.Contracts.Repositories;
using Boilerplate.Entities.DBModels;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Boilerplate.Repository.Repositories
{
    public class UserRepository : GenericRepository<AspNetUser>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<DataSet> GetAllUser(DataTableParams dataTableParams)
        {
            try
            {
                string query = @"exec [prc_GetAllUser] @PageIndex,@PageSize,@SearchText";

                var selector = new
                {
                    PageIndex = dataTableParams.PageIndex == 0 ? 1 : dataTableParams.PageIndex,
                    PageSize = dataTableParams.PageSize,
                    SearchText = dataTableParams.SearchText.Replace("'", "")
                };

                DataSet ds = await GetDataInDataSetAsync(query, selector);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ActiveInactive(string userId, bool isActive)
        {
            string query = @"UPDATE [dbo].[tblUserControl] 
                            SET [isActive] = @isActive 
                            WHERE UserId = @UserId";

            int rowAffect = await ExecuteAsync(query, new { isActive = !isActive, UserId = userId });

            return rowAffect > 0;
        }

        public async Task<bool> ResetPassword(string passwordhash,string userId)
        {
            string query = @"UPDATE [dbo].[AspNetUsers] 
                            SET [PasswordHash] = @Passwordhash 
                            WHERE Id = @UserId";

            int rowAffect = await ExecuteAsync(query, new { Passwordhash = passwordhash, UserId = userId });

            return rowAffect > 0;
        }

        public async Task<bool> CheckPassword(string passwordhash)
        {
            string query = @"SELECT * FROM [dbo].[AspNetUsers] WHERE PasswordHash = @Passwordhash ";

            DataTable dt= await GetDataInDataTableAsync(query, new { Passwordhash = passwordhash });

            return dt.Rows.Count > 0;
        }

        public async Task<DataSet> GetUserBasicData()
        {
            string sql = @"exec [prc_GetUserBasicData]";

            DataSet ds = await GetDataInDataSetAsync(sql, null);

            return ds;
        }

        public async Task<DataTable> GetUserAutoId()
        {
            string sql = @"select AutoId from dbo.vwUserIdGenerate";

            var dt = await GetDataInDataTableAsync(sql, null);

            return dt;
        }

        public async Task<bool> SaveUser(AspNetUserDto asp, UserControlDto tbluser, List<PagewiseActionDto> pagewiseActions)
        {
            string queryAspNetUsers = @"insert into AspNetUsers
            (Id, Email, EmailConfirmed, PasswordHash, UserName,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount,PasswordPin)
            values 
            (@Id, @Email, @EmailConfirmed, @PasswordHash, @UserName,0,0,0,0,@PasswordPin)";

            string querytblUserControl = @"insert into tblUserControl
            (UserId,Id,FullName,UserTypeId,MenuId,MakeBy,MakeDate,isActive,DashboardPreview)
            values
            (@UserId,@Id,@FullName,@UserTypeId,@MenuId,@MakeBy,@MakeDate,@isActive,@DashboardPreview)";

            using (SqlConnection con = new SqlConnection(_connectionStringTransactionDB))
            {
                con.Open();
                using (SqlTransaction trn = con.BeginTransaction())
                {
                    try
                    {
                        var rowAffectAspNetUsers = await ExecuteAsync(query: queryAspNetUsers, con: con, trn: trn, selector: asp);
                        if (!(rowAffectAspNetUsers > 0)) throw new Exception();

                        var rowAffectblUserControl = await ExecuteAsync(query: querytblUserControl, con: con, trn: trn, selector: tbluser);
                        if (!(rowAffectblUserControl > 0)) throw new Exception();

                        var rowAffect = await SavetblPagewiseActions(pagewiseActions, con, trn);
                        if (!(rowAffect > 0)) throw new Exception();

                        trn.Commit();
                        con.Close();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        trn.Rollback();
                        con.Close();
                        throw ex;
                    }
                }
            }
        }
        
        public async Task<bool> EditUser(string menu, UserCreate model, List<PagewiseActionDto> pagewiseActions)
        {
            string querytblUserControl = @"update tblUserControl set MenuId=@menu,DashboardPreview=@DashboardPreview where UserId=@UserId";

            string querytblPagewiseAction = @"delete from tblPagewiseAction
                where UserId = @UserId";

            using (SqlConnection con = new SqlConnection(_connectionStringTransactionDB))
            {
                con.Open();
                using (SqlTransaction trn = con.BeginTransaction())
                {
                    try
                    {
                        var rowAffecttblUserControl = await ExecuteAsync(query: querytblUserControl, con: con, trn: trn, selector: new { menu = menu, DashboardPreview = model.DashboardPreview, UserId = model.UserId });
                        if (!(rowAffecttblUserControl > 0)) throw new Exception();

                        var rowAffecttblPagewiseAction = await ExecuteAsync(query: querytblPagewiseAction, con: con, trn: trn, selector: new { UserId = model.UserId });
                        if (!(rowAffecttblPagewiseAction > 0)) throw new Exception();

                        var rowAffect = await SavetblPagewiseActions(pagewiseActions, con, trn);
                        if (!(rowAffect > 0)) throw new Exception();

                        trn.Commit();
                        con.Close();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        trn.Rollback();
                        con.Close();
                        throw ex;
                    }
                }
            }
        }

        public async Task<bool> DeleteUser(string userId)
        {
            string query = @"delete from tblPagewiseAction
                where UserId = @UserId

                declare  @Id varchar(128)
                select @Id = Id
                from tblUserControl
                where UserId = @UserId

                delete from tblUserControl
                where UserId = @UserId

                delete from AspNetUsers
                where Id = @Id";


            using (SqlConnection con = new SqlConnection(_connectionStringTransactionDB))
            {
                con.Open();
                using (SqlTransaction trn = con.BeginTransaction())
                {
                    try
                    {
                        var rowAffect = await ExecuteAsync(query: query, con: con, trn: trn, selector: new { UserId = userId });
                        if (!(rowAffect > 0)) throw new Exception();

                        trn.Commit();
                        con.Close();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        trn.Rollback();
                        con.Close();
                        throw ex;
                    }
                }
            }
        }

        private async Task<int> SavetblPagewiseActions(List<PagewiseActionDto> pagewiseActions, SqlConnection con, SqlTransaction trn)
        {
            string querytblPagewiseAction = @"insert into tblPagewiseAction
            (ActionID,UserId,MenuId,ActionPermission)
            values
            (@ActionID,@UserId,@MenuId,@ActionPermission)";

            try
            {
                var rowAffect = await ExecuteAsync(query: querytblPagewiseAction, con: con, trn: trn, selector: pagewiseActions);
                return rowAffect;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataSet> GetUserEditData(string userId)
        {
            try
            {
                string query = @"EXEC [prc_GetUserEditData] @UserId";

                DataSet ds = await GetDataInDataSetAsync(query, new { UserId = userId });

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetButtonActionByActionPermission(string actionPermission)
        {
            try
            {
                string query = @"select Id from tblButtonAction where Id IN (select value from string_split(@ActionPermission,','))";

                DataTable dt = await GetDataInDataTableAsync(query, new { ActionPermission = actionPermission });

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
