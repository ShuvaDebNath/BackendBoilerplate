using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Contracts.Repositories;

public interface IMasterEntryRepository
{
    bool ExecuteWriteOperation(string sqlQuery);
    DataTable ExecuteReadOperation(string sqlQuery);
}
