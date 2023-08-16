using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boilerplate.Repository.Repositories;
using Boilerplate.Repository.Interfaces;
using Microsoft.SqlServer.Management.Smo.Wmi;

namespace Boilerplate.Repository
{
    public static class DataAccessRegistration
    {
        public static void AddDataAccessLayer(this IServiceCollection service)
        {
            service.AddTransient<IAuthRepository, AuthRepository>();
            service.AddTransient<ICreateUserRepository, CreateUserRepository>();
            service.AddTransient<IMasterEntryRepository, MasterEntryRepository>();
            service.AddTransient<IDoubleMasterEntryRepository, DoubleMasterEntryRepository>();
            service.AddScoped<IGetDataRepository, GetDataRepository>();
        }
    }
}
