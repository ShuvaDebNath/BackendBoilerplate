using Boilerplate.Entities.DBModels;
using Boilerplate.Service.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Service.Interfaces
{
    public interface IDoubleMasterEntryService
    {
        public Task<Messages> SaveData(DoubleMasterEntryModel model, string authUserName);
        public Task<Messages> SaveListData(DoubleMasterEntryModel model, string authUserName);
        public Task<Messages> UpdateData(DoubleMasterEntryModel model, string authUserName);
        public Task<Messages> DeleteData(DoubleMasterEntryModel model);
    }
}
