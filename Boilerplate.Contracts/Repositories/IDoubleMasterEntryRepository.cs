namespace Boilerplate.Contracts.Repositories;
public interface IDoubleMasterEntryRepository
{
    public Task<int> SaveData(DoubleMasterEntryModel model, string AuthUserName);
    public Task<int> SaveListData(DoubleMasterEntryModel model, string AuthUserName);
    public Task<int> UpdateData(DoubleMasterEntryModel model, string AuthUserName);
    public Task<int> DeleteData(DoubleMasterEntryModel model);
}
