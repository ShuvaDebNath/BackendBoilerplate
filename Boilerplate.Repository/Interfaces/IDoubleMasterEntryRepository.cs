using Boilerplate.Entities.DBModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Repository.Interfaces
{
    public interface IDoubleMasterEntryRepository
    {
        public Task<int> SaveData(DoubleMasterEntryModel model, string AuthUserName);
        public Task<int> SaveListData(DoubleMasterEntryModel model, string AuthUserName);
        public Task<int> UpdateData(DoubleMasterEntryModel model, string AuthUserName);
        public Task<int> DeleteData(DoubleMasterEntryModel model);
    }
}
