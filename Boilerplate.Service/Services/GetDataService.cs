using Boilerplate.Contracts;
using Boilerplate.Contracts.Repositories;
using Boilerplate.Contracts.Responses;
using Boilerplate.Contracts.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;

namespace Boilerplate.Service.Services
{
    public class GetDataService : IGetDataService
    {
        IGetDataRepository _getDataRepository;
        private readonly ILogger<GetDataService> _logger;
        public GetDataService(IGetDataRepository getDataRepository,ILogger<GetDataService> logger)
        {
            _getDataRepository = getDataRepository;
            _logger = logger;
        }
        public async Task<Messages> GetAllData(GetDataModel model)
        {
            try
            {
                DataTable dt = await _getDataRepository.GetAllData(model);

                if (dt != null)
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
                _logger.LogInformation($"Source: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw;
            }
        }

        public async Task<Messages> GetDataById(GetDataModel model)
        {
            try
            {
                DataTable dt = await _getDataRepository.GetDataById(model);

                if (dt != null)
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
                _logger.LogInformation($"Source: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw;
            }
        }

        public async Task<Messages> GetInitialData(GetDataModel model)
        {
            try
            {
                DataSet ds = await _getDataRepository.GetInitialData(model);
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        ds.Tables[i].TableName = $"Tables{i + 1}";
                    }
                    var data = JsonConvert.SerializeObject(ds);
                    _logger.LogInformation($"Data Found!");
                    return MessageType.DataFound(data);
                }
                _logger.LogInformation($"Data Not Found!");
                return MessageType.NotFound(null);
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Source: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw;
            }
        }
    }
}
