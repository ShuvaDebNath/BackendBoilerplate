using Boilerplate.Contracts;
using Boilerplate.Contracts.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Boilerplate.Repository.Repositories
{
    public class DoubleMasterEntryRepository : GenericRepository<DoubleMasterEntryModel>, IDoubleMasterEntryRepository
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        public DoubleMasterEntryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> DeleteData(DoubleMasterEntryModel model)
        {
            string? strWhereParams = Convert.ToString(model.WhereParams);
            var WhereParams = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(strWhereParams);

            int cnt = ((IEnumerable<dynamic>)WhereParams).Count();
            var deleteMasterSQL = new StringBuilder($"DELETE FROM {model.TableNameMaster} WHERE ");
            var deleteChildSQL = new StringBuilder($"DELETE FROM {model.TableNameChild} WHERE ");

            var strWhereClouse = new StringBuilder();
            List<SqlParameter> deleteParam = new List<SqlParameter>();
            foreach (var item in (IEnumerable<dynamic>)WhereParams)
            {
                cnt--;
                if (cnt == 0)
                {
                    strWhereClouse.Append(item.Name + " = @" + item.Name);
                    deleteParam.Add(new SqlParameter($"@{item.Name}", item.Value.ToString()));
                    continue;
                }
                strWhereClouse.Append(item.Name + " = @" + item.Name + " AND ");
                deleteParam.Add(new SqlParameter($"@{item.Name}", item.Value.ToString()));
            }
            deleteMasterSQL.Append(strWhereClouse);
            deleteChildSQL.Append(strWhereClouse);

            int rowAffect = 0;
            conn = new SqlConnection(_connectionStringTransactionDB);

            if (conn.State != ConnectionState.Open)
                conn.Open();
            var trn = conn.BeginTransaction();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.Transaction = trn;
            cmd.Parameters.Clear();
            try
            {
                cmd.CommandText = deleteChildSQL.ToString();
                cmd.Parameters.AddRange(deleteParam.ToArray());
                rowAffect = await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = "";
                cmd.CommandText = deleteMasterSQL.ToString();
                rowAffect += await cmd.ExecuteNonQueryAsync();

                await cmd.Transaction.CommitAsync();
            }
            catch (Exception)
            {
                await cmd.Transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
            return rowAffect > 0 ? 1 : 0;
        }

        public async Task<int> SaveData(DoubleMasterEntryModel model, string authUserName)
        {
            int rowAffect = 0;
            conn = new SqlConnection(_connectionStringTransactionDB);

            if (conn.State != ConnectionState.Open)
                conn.Open();
            var trn = conn.BeginTransaction();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.Transaction = trn;
            try
            {
                string primeryKey = Guid.NewGuid().ToString();

                int serialNo = string.IsNullOrEmpty(model.ColumnNameSerialNo) ? 0 : await GenSerialNumberAsync(model.SerialType);

                rowAffect = await MasterTabelInsert(model, primeryKey, cmd, authUserName, serialNo);

                if (rowAffect > 0)
                {
                    rowAffect += await DetailsTabelInsert(model, primeryKey, cmd);
                }

                await cmd.Transaction.CommitAsync();
            }
            catch (Exception)
            {
                await cmd.Transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
            return rowAffect;

        }

        public async Task<int> SaveListData(DoubleMasterEntryModel model, string authUserName)
        {
            int rowAffect = 0;
            conn = new SqlConnection(_connectionStringTransactionDB);

            if (conn.State != ConnectionState.Open)
                conn.Open();
            var trn = conn.BeginTransaction();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.Transaction = trn;
            try
            {
                string commonKey = Guid.NewGuid().ToString();

                rowAffect += await MasterTabelListInsert(model, commonKey, cmd, authUserName);

                await cmd.Transaction.CommitAsync();
            }
            catch (Exception)
            {
                await cmd.Transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
            return rowAffect;
        }

        public async Task<int> UpdateData(DoubleMasterEntryModel model, string authUserName)
        {
            int rowAffect = 0;
            conn = new SqlConnection(_connectionStringTransactionDB);

            if (conn.State != ConnectionState.Open)
                conn.Open();
            var trn = conn.BeginTransaction();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.Transaction = trn;
            try
            {
                rowAffect = await MasterTabelUpdate(model, cmd, authUserName);

                await cmd.Transaction.CommitAsync();
            }
            catch (Exception)
            {
                await cmd.Transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
            return rowAffect;
        }
        private async Task<int> MasterTabelInsert(DoubleMasterEntryModel model, string primeryKey, SqlCommand cmd, string authUserName, int serialNo)
        {
            string? masterTablename = model.TableNameMaster;
            var sqlMaster = new StringBuilder($"insert into {masterTablename} (");
            var values = new StringBuilder(" values (");

            string? strdata = Convert.ToString(model.Data);

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(strdata);
            var cnt = ((IEnumerable<dynamic>)data).Count();
            cmd.Parameters.Clear();
            List<SqlParameter> param = new List<SqlParameter>();
            foreach (var item in (IEnumerable<dynamic>)data)
            {
                cnt--;
                if (cnt > 0)
                {
                    sqlMaster.Append(item.Name + ",");
                    values.Append("@" + item.Name + ",");
                }
                else
                {
                    sqlMaster.Append(item.Name);
                    values.Append("@" + item.Name);
                }
                if (item.Name.ToLower() == model.ColumnNamePrimary.ToLower())
                {
                    param.Add(new SqlParameter($"@{item.Name}", primeryKey));
                }
                else if (item.Name.ToLower() == model.ColumnNameSerialNo.ToLower())
                {
                    if (serialNo > 0)
                    {
                        param.Add(new SqlParameter($"@{item.Name}", item.Value.ToString() + "" + serialNo.ToString()));
                    }
                    else
                    {
                        param.Add(new SqlParameter($"@{item.Name}", item.Value.ToString()));
                    }
                }
                else
                {
                    param.Add(new SqlParameter($"@{item.Name}", item.Value.ToString()));
                }
            }
            sqlMaster.Append(", MakeDate, MakeBy, InsertTime)");
            values.Append(", getdate(), @authUserName , getdate())");
            param.Add(new SqlParameter("@authUserName", authUserName));
            sqlMaster.Append(values.ToString());
            cmd.CommandText = sqlMaster.ToString();
            cmd.Parameters.AddRange(param.ToArray());
            return await cmd.ExecuteNonQueryAsync();
        }
        private async Task<int> MasterTabelUpdate(DoubleMasterEntryModel model, SqlCommand cmd, string authUserName)
          {
            string? masterTablename = model.TableNameMaster;
            string primeryKey = "";
            cmd.Parameters.Clear();
            List<SqlParameter> param = new List<SqlParameter>();

            string? strdata = Convert.ToString(model.Data);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(strdata);

            string? strWhereParams = Convert.ToString(model.WhereParams);
            var WhereParams = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(strWhereParams);

            var sqlMaster = new StringBuilder($"UPDATE {masterTablename} SET ");
            foreach (var item in (IEnumerable<dynamic>)data)
            {
                if (item.Name.ToLower() != model.ColumnNamePrimary.ToLower())
                {
                    sqlMaster.Append(item.Name + " = @" + item.Name + ",");
                    param.Add(new SqlParameter($"@{item.Name}", item.Value.ToString()));
                }

            }
            sqlMaster.Append("UpdateDate = getdate(), UpdateBy = @authUserName,UpdateTime=getdate() WHERE ");
            param.Add(new SqlParameter("@authUserName", authUserName));
            int cnt = ((IEnumerable<dynamic>)WhereParams).Count();
            var deleteSQL = new StringBuilder($"DELETE FROM {model.TableNameChild} WHERE ");
            var strWhereClouse = new StringBuilder();
            List<SqlParameter> whereParam = new List<SqlParameter>();

            foreach (var item in (IEnumerable<dynamic>)WhereParams)
            {
                primeryKey = item.Name.ToString().ToLower() == model.ColumnNamePrimary.ToLower() ? item.Value.ToString() : primeryKey;
                cnt--;
                if (cnt == 0)
                {
                    strWhereClouse.Append(item.Name + " = @" + item.Name);
                    whereParam.Add(new SqlParameter($"@{item.Name}", item.Value.ToString()));
                    continue;
                }
                strWhereClouse.Append(item.Name + " = @" + item.Name + " AND ");
                whereParam.Add(new SqlParameter($"@{item.Name}", item.Value.ToString()));
            }
            sqlMaster.Append(strWhereClouse.ToString());
            deleteSQL.Append(strWhereClouse.ToString());
            cmd.CommandText = sqlMaster.ToString();
            cmd.Parameters.AddRange(param.ToArray());
            cmd.Parameters.AddRange(whereParam.ToArray());
            int rowAffect = await cmd.ExecuteNonQueryAsync();

            if (rowAffect > 0)
            {
                cmd.CommandText = "";
                cmd.CommandText = deleteSQL.ToString();
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(whereParam.ToArray());
                rowAffect += await cmd.ExecuteNonQueryAsync();

                rowAffect += await DetailsTabelInsert(model, primeryKey, cmd);
            }

            return rowAffect > 0 ? 1 : 0;

        }
        private async Task<int> DetailsTabelInsert(DoubleMasterEntryModel model, string primeryKey, SqlCommand cmd)
        {
            int rowAffect = 0;
            string? childTablename = model.TableNameChild;

            string? strdata = Convert.ToString(model.DetailsData);
            var details = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(strdata);


            foreach (var citem in (IEnumerable<dynamic>)details)
            {
                var cData = citem;
                int cnt = ((IEnumerable<dynamic>)cData).Count();

                var sqlChiled = new StringBuilder($" insert into {childTablename} (");
                var values = new StringBuilder("values (");
                cmd.Parameters.Clear();
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (var j in (IEnumerable<dynamic>)cData)
                {
                    cnt--;

                    if (cnt > 0)
                    {
                        sqlChiled.Append(j.Name + ",");
                        if (j.Value.ToString().ToLower() == "newid()")
                            values.Append(j.Value.ToString() + ",");
                        else
                            values.Append("@" + j.Name + ",");
                    }
                    else
                    {
                        sqlChiled.Append(j.Name);
                        if (j.Value.ToString().ToLower() == "newid()")
                            values.Append("'" + j.Value.ToString() + "'");
                        else
                            values.Append("@" + j.Name);
                    }
                    if (j.Value.ToString().ToLower() != "newid()")
                    {
                        if (j.Name.ToLower() == model.ColumnNamePrimary.ToLower())
                        {
                            parameters.Add(new SqlParameter("@" + j.Name, primeryKey));
                        }
                        else
                        {
                            parameters.Add(new SqlParameter("@" + j.Name, j.Value.ToString()));
                        }
                    }
                }
                sqlChiled.Append(") ");
                values.Append(") ");
                sqlChiled.Append(values.ToString());
                cmd.CommandText = sqlChiled.ToString();
                cmd.Parameters.AddRange(parameters.ToArray());
                rowAffect += await cmd.ExecuteNonQueryAsync();
            }
            return rowAffect > 0 ? 1 : 0;
        }
        private async Task<int> MasterTabelListInsert(DoubleMasterEntryModel model, string commonKey, SqlCommand cmd, string authUserName)
        {
            int rowAffect = 0;
            string? childTablename = model.TableNameChild;

            string? strdata = Convert.ToString(model.DetailsData);
            var details = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(strdata);

            foreach (var citem in (IEnumerable<dynamic>)details)
            {
                var cData = citem;
                int cnt = ((IEnumerable<dynamic>)cData).Count();

                var sqlChiled = new StringBuilder($" insert into {childTablename} (");
                var values = new StringBuilder("values (");
                cmd.Parameters.Clear();
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (var j in (IEnumerable<dynamic>)cData)
                {
                    cnt--;

                    if (cnt > 0)
                    {
                        sqlChiled.Append(j.Name + ",");
                        if (j.Value.ToString().ToLower() == "newid()")
                            values.Append(j.Value.ToString() + ",");
                        else
                            values.Append("@" + j.Name + ",");
                    }
                    else
                    {
                        sqlChiled.Append(j.Name);
                        if (j.Value.ToString().ToLower() == "newid()")
                            values.Append(j.Value.ToString());
                        else
                            values.Append("@" + j.Name);
                    }
                    if (j.Value.ToString().ToLower() != "newid()")
                    {
                        parameters.Add(new SqlParameter("@" + j.Name, j.Value.ToString()));
                    }
                }
                sqlChiled.Append(", MakeDate, MakeBy, InsertTime)");
                values.Append(", getdate(), @authUserName , getdate())");
                parameters.Add(new SqlParameter("@authUserName", authUserName));
                sqlChiled.Append(values.ToString());
                cmd.CommandText = sqlChiled.ToString();
                cmd.Parameters.AddRange(parameters.ToArray());
                rowAffect += await cmd.ExecuteNonQueryAsync();
            }
            return rowAffect > 0 ? 1 : 0;
        }
    }
}
