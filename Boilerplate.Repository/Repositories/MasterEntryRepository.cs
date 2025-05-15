using Boilerplate.Contracts;
using Boilerplate.Contracts.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Boilerplate.Repository.Repositories
{
   public class MasterEntryRepository : GenericRepository<MasterEntryModel>, IMasterEntryRepository
    {
        public MasterEntryRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public bool ExecuteWriteOperation(string sqlQuery)
        {
            using var sqlConnection = new SqlConnection(_connectionStringTransactionDB);
            var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

            sqlConnection.Open();
            var isExecuted = sqlCommand.ExecuteNonQuery();
            sqlConnection.Dispose();

            return isExecuted > 0;
        }

        public DataTable ExecuteReadOperation(string sqlQuery)
        {
            using var sqlConnection = new SqlConnection(_connectionStringTransactionDB);
            var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

            if (sqlConnection.State!=ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            sqlConnection.Dispose();

            return dataTable;
        }
    }
}
