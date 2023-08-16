using Boilerplate.Entities.DBModels;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Boilerplate.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAllAsync(string query, object? param = null);
        Task<int> GetCountAsync(string tableName, string columnName, dynamic columnData);
        Task<TResult> GetAllSingleAsync<TResult>(string query, object? param = null);
        Task<DataTable> GetDataInDataTableAsync(string query, object selector);
        Task<int> ExecuteAsync(string query, SqlConnection con, DbTransaction trn, object? selector = null);
        Task<int> ExecuteAsync(string query, object? selector = null);
        Task<int> GenSerialNumberAsync(string SerialType);
    }
}
