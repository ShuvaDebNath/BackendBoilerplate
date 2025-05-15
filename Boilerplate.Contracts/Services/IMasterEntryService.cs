using Boilerplate.Contracts.Responses;

namespace Boilerplate.Contracts.Services;

public interface IMasterEntryService
{
    Messages GetAll(MasterEntryModel item);
    Messages GetByColumns(MasterEntryModel item);
    Messages Insert(MasterEntryModel item, string userName);
    Messages Update(MasterEntryModel item, string userName);
    Messages Delete(MasterEntryModel item);
}
