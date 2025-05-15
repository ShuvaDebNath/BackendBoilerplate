using Boilerplate.Contracts.Repositories;
using Boilerplate.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Repository;

public static class DataAccessRegistration
{
    public static void AddDataAccessLayer(this IServiceCollection service)
    {
        service.AddTransient<IAuthRepository, AuthRepository>();
        service.AddTransient<IUserRepository, UserRepository>();
        service.AddTransient<IMasterEntryRepository, MasterEntryRepository>();
        service.AddTransient<IDoubleMasterEntryRepository, DoubleMasterEntryRepository>();
        service.AddScoped<IGetDataRepository, GetDataRepository>();
    }
}
