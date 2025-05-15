

using Boilerplate.Contracts.Responses;

namespace Boilerplate.Contracts.Services;

public interface IGetDataService
{
    public Task<Messages> GetInitialData(GetDataModel model);
    public Task<Messages> GetAllData(GetDataModel model);
    public Task<Messages> GetDataById(GetDataModel model);
}
