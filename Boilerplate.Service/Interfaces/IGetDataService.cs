using Boilerplate.Entities.DTOs;
using Boilerplate.Service.Message;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Service.Interfaces
{
    public interface IGetDataService
    {
        public Task<Messages> GetInitialData(GetDataModel model);
        public Task<Messages> GetAllData(GetDataModel model);
        public Task<Messages> GetDataById(GetDataModel model);
    }
}
