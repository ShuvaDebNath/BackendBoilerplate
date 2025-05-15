using Boilerplate.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Contracts.Repositories;

public interface IGetDataRepository
{
    public Task<DataSet> GetInitialData(GetDataModel model);
    public Task<DataTable> GetAllData(GetDataModel model);
    public Task<DataTable> GetDataById(GetDataModel model);
}
