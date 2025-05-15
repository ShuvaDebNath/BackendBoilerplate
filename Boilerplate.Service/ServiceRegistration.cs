
using Boilerplate.Contracts.Services;
using Boilerplate.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Service
{
    public static class ServiceRegistration
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMasterEntryService, MasterEntryService>();
            services.AddTransient<IDoubleMasterEntryService, DoubleMasterEntryService>();
            services.AddScoped<IGetDataService, GetDataService>();
            services.ConfigureCors();
        }

        private static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .WithMethods("GET", "POST", "UPDATE", "PUT", "DELETE")
                    .AllowAnyHeader());
            });

            return services;
        }
    }
}
