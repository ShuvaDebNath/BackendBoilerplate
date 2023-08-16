using Boilerplate.Entities.DTOs;
using Boilerplate.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Repository.Repositories
{
    public class GetDataRepository : GenericRepository<GetDataModel>, IGetDataRepository
    {
        private List<SqlParameter> parameters = new List<SqlParameter>();
        private SqlConnection conn;
        private SqlCommand cmd;

        public GetDataRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<DataTable> GetAllData(GetDataModel model)
        {
            try
            {
                StringBuilder executeQuery = new StringBuilder($"exec {model.ProcedureName}");
                executeQuery.Append(GenerateParamsQuery(model, ref parameters));

                DataTable dt = await GetDataInDataTableAsync(query: executeQuery.ToString(), selector: model.Parameters);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> GetDataById(GetDataModel model)
        {
            try
            {
                StringBuilder executeQuery = new StringBuilder($"exec {model.ProcedureName} ");
                executeQuery.Append(GenerateParamsQuery(model, ref parameters));
                conn = new SqlConnection(_connectionStringTransactionDB);

                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                cmd.CommandText = executeQuery.ToString();
                cmd.Parameters.AddRange(parameters.ToArray());
                DataTable dt = new DataTable();
                dt.Load(await cmd.ExecuteReaderAsync());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<DataSet> GetInitialData(GetDataModel model)
        {
            try
            {
                StringBuilder executeQuery = new StringBuilder($"exec {model.ProcedureName}");
                executeQuery.Append(GenerateParamsQuery(model,ref parameters));
                DataSet ds = await GetDataInDataSetAsync(query: executeQuery.ToString(), selector: model.Parameters);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private StringBuilder GenerateParamsQuery(GetDataModel model,ref List<SqlParameter> parameters)
        {
            if (model.Parameters != null)
            {
                StringBuilder allParams = new StringBuilder();
                string? strParams = Convert.ToString(model.Parameters);
                var param = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(strParams);
                var cnt = ((IEnumerable<dynamic>)param).Count();
                foreach (var item in (IEnumerable<dynamic>)param)
                {
                    cnt--;
                    if (cnt > 0)
                    {
                        allParams.Append("@" + item.Name + ",");
                    }
                    else
                    {
                        allParams.Append("@" + item.Name);
                    }
                    parameters.Add(new SqlParameter($"@{item.Name}", item.Value.ToString()));
                }
                return allParams;
            }
            return null;
        }
    }
}
