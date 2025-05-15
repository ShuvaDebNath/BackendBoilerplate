using Boilerplate.Contracts;
using Boilerplate.Contracts.Repositories;
using Boilerplate.Contracts.Responses;
using Boilerplate.Contracts.Services;
using Boilerplate.Service.ValidationHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Boilerplate.Service.Services
{
    public class MasterEntryService : IMasterEntryService
    {
        private readonly IMasterEntryRepository _masterEntryRepository;
        private readonly ILogger<MasterEntryService> _logger;
        private readonly IConfiguration _configuration;

        public MasterEntryService(IMasterEntryRepository masterEntryRepository, ILogger<MasterEntryService> logger, IConfiguration configuration)
        {
            _masterEntryRepository = masterEntryRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public Messages GetAll(MasterEntryModel item)
        {
            try
            {
                var sqlQuery = $" SELECT {item.ColumnNames} FROM [dbo].[{item.TableName}] ";

                var dt = _masterEntryRepository.ExecuteReadOperation(sqlQuery);

                if (dt != null && dt.Rows.Count > 0)
                {
                    //var data = JsonConvert.SerializeObject(dt);
                    var data = readyjsonFromDataTable(dt);
                    _logger.LogInformation($"Data Found!");
                    return MessageType.DataFound(data);
                }
                _logger.LogInformation($"Data Not Found!");
                return MessageType.NotFound(null);
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Sourc: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw;
            }
        }

        public Messages GetByColumns(MasterEntryModel item)
        {
            try
            {
                var sqlQuery = $" SELECT {item.ColumnNames} FROM [dbo].[{item.TableName}] ";

                sqlQuery += WhereParams(item);

                var dt = _masterEntryRepository.ExecuteReadOperation(sqlQuery);

                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = JsonConvert.SerializeObject(dt);
                    _logger.LogInformation($"Data Found!");
                    return MessageType.DataFound(data);
                }
                _logger.LogInformation($"Data Not Found!");
                return MessageType.NotFound(null);
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Sourc: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw;
            }
        }

        public Messages Insert(MasterEntryModel item, string userName)
        {
            try
            {
                var validationHelper = new ValidationHelper(_configuration);
                validationHelper.ValidateModel(item, item.TableName);

                var sqlQuery = $"INSERT INTO [dbo].[{item.TableName}] ";

                sqlQuery += InsertQueryGeneratorWithKeyParams(item);

                sqlQuery += InsertQueryGeneratorWithValueParams(item, userName);

                var result = _masterEntryRepository.ExecuteWriteOperation(sqlQuery);

                if (result)
                {
                    _logger.LogInformation($"Data Save Success!");
                    return MessageType.SaveSuccess(item);
                }
                _logger.LogInformation($"Data Save Fail!");
                return MessageType.SaveError(null);
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Sourc: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw;
            }
        }

        public Messages Update(MasterEntryModel item,string userName)
        {
            try
            {
                var sqlQuery = $"Update [dbo].[{item.TableName}] SET ";

                sqlQuery += QueryParams(item, userName);

                sqlQuery += WhereParams(item);

                var result = _masterEntryRepository.ExecuteWriteOperation(sqlQuery);

                if (result)
                {
                    _logger.LogInformation($"Data Update Success!");
                    return MessageType.UpdateSuccess(item);
                }
                _logger.LogInformation($"Data Update Fail!");
                return MessageType.UpdateError(null);
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Sourc: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw;
            }
        }

        public Messages Delete(MasterEntryModel item)
        {
            try
            {
                var sqlQuery = $" DELETE FROM [dbo].[{item.TableName}] ";

                sqlQuery += WhereParams(item);

                var result = _masterEntryRepository.ExecuteWriteOperation(sqlQuery);

                if (result)
                {
                    _logger.LogInformation($"Data Delete Success!");
                    return MessageType.DeleteSuccess(item);
                }
                _logger.LogInformation($"Data Delete Fail!");
                return MessageType.DeleteError(null);
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Sourc: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw;
            }
        }

        private string InsertQueryGeneratorWithKeyParams(MasterEntryModel item)
        {
            Object objQueryParams = new Object();

            objQueryParams = item.QueryParams;

            string strQueryParamsJsonData = Convert.ToString(objQueryParams);

            var dataQueryParams = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(strQueryParamsJsonData);

            var sqlQuery = " ( ";

            foreach (var itemQueryParams in dataQueryParams)
            {
                sqlQuery += " " + itemQueryParams.Key + ",";
            }

            sqlQuery += " MakeBy,MakeDate,InsertTime";

            return sqlQuery;
        }

        private string InsertQueryGeneratorWithValueParams(MasterEntryModel item,string userName)
        {
            Object objQueryParams = new Object();

            objQueryParams = item.QueryParams;

            string strQueryParamsJsonData = Convert.ToString(objQueryParams);

            var dataQueryParams = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(strQueryParamsJsonData);

            var sqlQuery = " ) VALUES (";

            foreach (var itemQueryParams in dataQueryParams)
            {
                if (itemQueryParams.Value.ToLower() == "newid()")
                {
                    sqlQuery += " " + itemQueryParams.Value + " ,";
                }
                else
                {
                    sqlQuery += " '" + itemQueryParams.Value + "' ,";
                }
            }

            sqlQuery += " '" + userName + "' ,";
            sqlQuery += " getdate() ,";
            sqlQuery += " getdate() ";

            sqlQuery += ")";

            return sqlQuery;
        }

        private string QueryParams(MasterEntryModel item,string userName)
        {
            var dataQueryParams = ConvertObjectToDictionaryForQueryParams(item);

            var sqlQuery = "";

            foreach (var itemQueryParams in dataQueryParams)
            {
                sqlQuery += " " + itemQueryParams.Key + " = '" + itemQueryParams.Value + "',";
            }

            sqlQuery += " UpdateBy = '" + userName + "',";
            sqlQuery += " UpdateDate = getdate(),";
            sqlQuery += " UpdateTime = getdate()";

            return sqlQuery;
        }

        private Dictionary<string, string> ConvertObjectToDictionaryForQueryParams(MasterEntryModel item)
        {
            Object objQueryParams = new Object();

            objQueryParams = item.QueryParams;

            string strQueryParamsJsonData = Convert.ToString(objQueryParams);

            var dataQueryParams = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(strQueryParamsJsonData);

            return dataQueryParams;
        }

        private string WhereParams(MasterEntryModel item)
        {
            var dataWhereParams = ConvertObjectToDictionaryWhereParams(item);

            var sqlQuery = " where ";

            foreach (var itemWhereParams in dataWhereParams)
            {
                sqlQuery += " " + itemWhereParams.Key + " = '" + itemWhereParams.Value + "' and ";
            }

            sqlQuery = sqlQuery.TrimEnd();

            if (sqlQuery.Contains("and"))
            {
                sqlQuery = sqlQuery.Substring(0, sqlQuery.LastIndexOf("and"));
            }

            return sqlQuery;

        }

        private Dictionary<string, string> ConvertObjectToDictionaryWhereParams(MasterEntryModel item)
        {
            Object objWhereParams = new Object();

            objWhereParams = item.WhereParams;

            string strWhereParamsJsonData = Convert.ToString(objWhereParams);

            var dataWhereParams = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(strWhereParamsJsonData);

            return dataWhereParams;
        }

        public string readyjsonFromDataTable(DataTable dt)
        {
            var JArr = new JArray();
            JObject tableObj = new JObject();
            dt.TableName = $"Table{1}";
            tableObj.Add("Table", dt.TableName.ToString());
            DataTable current = dt;
            JObject metaData = new JObject();
            int countMeta = 0;
            foreach (DataColumn column in current.Columns)
            {
                metaData.Add(countMeta.ToString(), column.ColumnName);
                countMeta++;
            }
            JArray dataRows = new JArray();
            foreach (DataRow row in current.Rows)
            {
                JArray rowData = new JArray();
                foreach (DataColumn column in current.Columns)
                {
                    rowData.Add(JToken.FromObject(row[column.ColumnName]));
                }
                dataRows.Add(rowData);
            }

            tableObj.Add("metadata", metaData);
            tableObj.Add("data", dataRows);
            var jsonData = JsonConvert.SerializeObject(tableObj, Formatting.Indented);
            JArr.Add(jsonData);

            var data = JsonConvert.SerializeObject(JArr);

            return data.Replace("\\\\r\\\\n  \\\\\\", "");
        }
    }
}
