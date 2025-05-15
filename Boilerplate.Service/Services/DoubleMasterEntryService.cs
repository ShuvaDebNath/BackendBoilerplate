
using Boilerplate.Contracts;
using Boilerplate.Contracts.Repositories;
using Boilerplate.Contracts.Responses;
using Boilerplate.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Service.Services;

public class DoubleMasterEntryService : IDoubleMasterEntryService
{
    readonly IDoubleMasterEntryRepository _doubleMasterEntryRepository;
    private readonly ILogger<DoubleMasterEntryService> _logger;
    public DoubleMasterEntryService(IDoubleMasterEntryRepository doubleMasterEntryRepository, 
        ILogger<DoubleMasterEntryService> logger)
    {
        _doubleMasterEntryRepository = doubleMasterEntryRepository;
        _logger = logger;
    }
    public async Task<Messages> DeleteData(DoubleMasterEntryModel model)
    {
        try
        {
            int rowAffect = await _doubleMasterEntryRepository.DeleteData(model);
            if (rowAffect > 0)
            {
                _logger.LogInformation($"Data Delete Success!");
                return MessageType.DeleteSuccess(model);
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

    public async Task<Messages> SaveData(DoubleMasterEntryModel model, string authUserName)
    {
        try
        {
            if (model.IsFlag != null)
            {
                bool isMemberFind = await IsMemberNotExist(model);

                if (isMemberFind == false)
                {
                    return MessageType.NotFound("Member Name Not Found!");
                }
                else
                {
                    return await SaveDataGateway(model, authUserName);
                }
            }
            else
            {
                return await SaveDataGateway(model, authUserName);
            }
           
        }
        catch (Exception ex)
        {
            string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            _logger.LogInformation($"Sourc: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
            throw;
        }
    }

    private async Task<Messages> SaveDataGateway(DoubleMasterEntryModel model, string authUserName)
    {
        int rowAffect = await _doubleMasterEntryRepository.SaveData(model, authUserName);
        if (rowAffect > 0)
        {
            _logger.LogInformation($"Data Save Success!");
            return MessageType.SaveSuccess(model);
        }
        _logger.LogInformation($"Data Save Fail!");
        return MessageType.SaveError(null);
    }

    public async Task<Messages> UpdateData(DoubleMasterEntryModel model, string authUserName)
    {
        try
        {
            if (model.IsFlag != null)
            {
                bool isMemberFind = await IsMemberNotExist(model);

                if (isMemberFind == false)
                {
                    return MessageType.NotFound("Member Name Not Found!");
                }
                else
                {
                    return await UpdateDataGateway(model, authUserName);
                }
            }
            else
            {
                return await UpdateDataGateway(model, authUserName);
            }

        }
        catch (Exception ex)
        {
            string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            _logger.LogInformation($"Sourc: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
            throw;
        }
    }

    private async Task<Messages> UpdateDataGateway(DoubleMasterEntryModel model, string authUserName)
    {
        int rowAffect = await _doubleMasterEntryRepository.UpdateData(model, authUserName);
        if (rowAffect > 0)
        {
            _logger.LogInformation($"Data Update Success!");
            return MessageType.UpdateSuccess(model);
        }
        _logger.LogInformation($"Data Update Fail!");
        return MessageType.UpdateError(null);
    }

    public async Task<Messages> SaveListData(DoubleMasterEntryModel model, string authUserName)
    {
        try
        {
            int rowAffect = await _doubleMasterEntryRepository.SaveListData(model, authUserName);
            if (rowAffect > 0)
            {
                _logger.LogInformation($"Data Save Success!");
                return MessageType.SaveSuccess(model);
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

    private async Task<bool> IsMemberNotExist(DoubleMasterEntryModel model)
    {
        if (model.IsFlag.ToLower() == "Member_Check_for_Sales".ToLower())
        {
            string memberId = "";
            if (IsKeyExist(model.Data, "MemberId", out memberId))
            {   
                return false;
                //if (!(await _isDataCheck.IsMemberExist(memberId)))
                //{
                    
                //}
            }
        }
        return true;
    }
    private bool IsKeyExist(object data,string keyName,out string memberId)
    {
        string lKeyName = keyName.ToLower();
        string? strdata = Convert.ToString(data);
        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(strdata);
        var tt=((IEnumerable<dynamic>)obj).Where(x => x.Name.ToString().ToLower() == lKeyName);
        memberId = tt.Any() ? tt.FirstOrDefault().Value.ToString() : "";
        return tt.Any();
    }
}
