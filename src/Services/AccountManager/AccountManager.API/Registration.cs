using AccountManager.API.Filters;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManager.API
{
    public static class Registration
    {
        public static IMvcBuilder RegisterApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Profile, AutoMapperProfile>();

            services.AddCors();

            return services.AddMvc(config =>
            {
                config.Filters.Add<KnownExceptionFilter>();
                config.Filters.Add<OperationCancelledExceptionFilter>();
            });
        }
    }
}
