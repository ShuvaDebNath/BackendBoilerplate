using Boilerplate.Entities.DBModels;
using Boilerplate.Service.Message;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Service.Interfaces
{
    public interface IMasterEntryService
    {
        Messages GetAll(MasterEntryModel item);
        Messages GetByColumns(MasterEntryModel item);
        Messages Insert(MasterEntryModel item, string userName);
        Messages Update(MasterEntryModel item, string userName);
        Messages Delete(MasterEntryModel item);
    }
}
