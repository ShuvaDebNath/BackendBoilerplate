

using Boilerplate.Contracts.Responses;

namespace Boilerplate.Contracts.Services;

public interface IDoubleMasterEntryService
{
    public Task<Messages> SaveData(DoubleMasterEntryModel model, string authUserName);
    public Task<Messages> SaveListData(DoubleMasterEntryModel model, string authUserName);
    public Task<Messages> UpdateData(DoubleMasterEntryModel model, string authUserName);
    public Task<Messages> DeleteData(DoubleMasterEntryModel model);
}
