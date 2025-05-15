using Boilerplate.Contracts.Repositories;
using Boilerplate.Entities.DBModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Boilerplate.Repository
{

    public abstract class GenericRepository<TDto> : IGenericRepository<TDto> where TDto : class
    {
        public readonly string _connectionStringTransactionDB;
        public readonly string _connectionStringUserDB;

        public GenericRepository(IConfiguration configuration)
        {
            _connectionStringTransactionDB = configuration.GetConnectionString("TransactionDBConnection");
            _connectionStringUserDB = configuration.GetConnectionString("UserDBConnection");
        }

        public async Task<IList<TDto>> GetAllAsync(string query, object? param = null)
        {
            using var connection = new SqlConnection(_connectionStringTransactionDB);

            var result = await connection.QueryAsync<TDto>(query, param);

            return result.ToList();
        }

        public async Task<TResult> GetAllSingleAsync<TResult>(string query, object? param = null)
        {
            using var connection = new SqlConnection(_connectionStringTransactionDB);
            var result = await connection.QueryAsync<TResult>(query, param);

            return result.FirstOrDefault();
        }

        public async Task<int> GetCountAsync(string tableName, string columnName, dynamic columnData)
        {
            using var connection = new SqlConnection(_connectionStringTransactionDB);
            string query = "select Count(" + columnName + ") from " + tableName + " where " + columnName + " = @ColumnData ";
            var result = await connection.QueryAsync<int>(query, new { ColumnData = columnData });

            return result.FirstOrDefault();
        }

        public async Task<DataTable> GetDataInDataTableAsync(string query, object? selector = null)
        {
            using var connection = new SqlConnection(_connectionStringTransactionDB);

            DataTable table = new DataTable();
            table.Load(await connection.ExecuteReaderAsync(query, selector));

            return table;
        }

        public async Task<DataSet> GetDataInDataSetAsync(string query, object selector)
        {
            DataSet dsList = new DataSet();

            using (var connection = new SqlConnection(_connectionStringTransactionDB))
            {
                IDataReader ds = await connection.ExecuteReaderAsync(query, selector);
                dsList = ConvertDataReaderToDataSet(ds);
            }
            return dsList;
        }

        private DataSet ConvertDataReaderToDataSet(IDataReader dataReader)
        {
            DataSet ds = new DataSet();
            int i = 0;
            while (!dataReader.IsClosed)
            {
                ds.Tables.Add("dt" + (i + 1));
                ds.EnforceConstraints = false;

                try
                {
                    ds.Tables[i].Load(dataReader);
                    i++;
                }
                catch (Exception ex)
                {
                   throw new Exception(ex.Message);
                }
            }
            return ds;
        }

        public async Task<int> ExecuteAsync(string query, SqlConnection con, DbTransaction trn, object? selector = null)
        {
            try
            {
                var affectedRows = await con.ExecuteAsync(query, selector, trn);

                try
                {
                    await TransactionHistory(query, JsonConvert.SerializeObject(selector), "", "", "", "");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return affectedRows;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> ExecuteAsync(string query, object? selector = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionStringTransactionDB))
                {
                    var affectedRows = await connection.ExecuteAsync(query, selector);

                    try
                    {
                        await TransactionHistory(query, JsonConvert.SerializeObject(selector), "", "", "", "");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<int> TransactionHistory(string sqlQuery, string queryValue, string userId, string ipAddress, string macAddress, string pcName)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[TransactionHistory]
           ([TransactionType]
           ,[SqlQuery]
           ,[QueryValue]
           ,[Date]
           ,[UserId]
           ,[IPAddress]
           ,[MacAddress]
           ,[PCName])
     VALUES
           (@TransactionType
           ,@SqlQuery
           ,@QueryValue
           ,getdate()
           ,@UserId
           ,@IPAddress
           ,@MacAddress
           ,@PCName)";

                string transactionType = "";
                if (sqlQuery.ToLower().Contains("insert"))
                {
                    transactionType = "insert";
                }
                else if (sqlQuery.ToLower().Contains("update"))
                {
                    transactionType = "update";
                }
                else if (sqlQuery.ToLower().Contains("delete"))
                {
                    transactionType = "delete";
                }

                var selector = new
                {
                    TransactionType = transactionType,
                    SqlQuery = sqlQuery,
                    QueryValue = queryValue,
                    UserId = userId,
                    IPAddress = ipAddress,
                    MacAddress = macAddress,
                    PCName = pcName,
                };

                using (var connection = new SqlConnection(_connectionStringTransactionDB))
                {
                    var affectedRows = await connection.ExecuteAsync(query, selector);
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GenSerialNumberAsync(string? SerialType)
        {
            SerialNoGenerate model = new()
            {
                SerialType = SerialType,
                SerialId = Guid.NewGuid().ToString()
            };
            try
            {                
                StringBuilder maxNumberQuery = new(@"(SELECT ISNULL(MAX(SerialNo),0) AS SerialNo FROM SerialNoGenerate WHERE SerialType = @SerialType)");

                using (var connection = new SqlConnection(_connectionStringTransactionDB))
                {
                    await connection.OpenAsync();
                    var data = await connection.QueryAsync<SerialNoGenerate>(maxNumberQuery.ToString(), new { SerialType = model.SerialType });
                    if (data.Any())
                    {
                        var item = data.FirstOrDefault();
                        if (item != null)
                        {
                            item.SerialNo += 1;
                            model.SerialNo = item.SerialNo;
                            using (var trn = await connection.BeginTransactionAsync())
                            {
                                try
                                {
                                    StringBuilder sql = new();
                                    if (item.SerialNo <= 1)
                                    {
                                        sql.Append(@"INSERT INTO [dbo].[SerialNoGenerate]([SerialId],[SerialType],[SerialNo])
                                                VALUES(@SerialId,@SerialType,@SerialNo)");
                                        await connection.ExecuteAsync(sql.ToString(), model, trn);
                                    }
                                    else
                                    {
                                        sql.Append(@"UPDATE [dbo].[SerialNoGenerate]
                                                set [SerialId]=@SerialId,
                                                    [SerialNo]=@SerialNo WHERE [SerialType]=@SerialType");
                                        await connection.ExecuteAsync(sql.ToString(), model, trn);
                                    }
                                    await trn.CommitAsync();
                                }
                                catch (Exception)
                                {
                                    await trn.RollbackAsync();
                                    throw;
                                }
                                finally
                                {
                                    await connection.CloseAsync();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return model.SerialNo;
        }

        public async Task<IList<TResult>> GetAllAsync<TResult>(string query, object? param = null) where TResult : class
        {
            using var connection = new SqlConnection(_connectionStringTransactionDB);

            return (await connection.QueryAsync<TResult>(query, param)).ToList();
        }
    }
}
