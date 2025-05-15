using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Boilerplate.Contracts.Repositories;

public interface IGenericRepository<TDto> where TDto : class
{
    Task<IList<TDto>> GetAllAsync(string query, object? param = null);
    Task<int> GetCountAsync(string tableName, string columnName, dynamic columnData);
    Task<TResult> GetAllSingleAsync<TResult>(string query, object? param = null);
    Task<DataTable> GetDataInDataTableAsync(string query, object selector);
    Task<int> ExecuteAsync(string query, SqlConnection con, DbTransaction trn, object? selector = null);
    Task<int> ExecuteAsync(string query, object? selector = null);
    Task<int> GenSerialNumberAsync(string SerialType);
    Task<IList<TResult>> GetAllAsync<TResult>(string query, object? param = null) where TResult : class;
}
