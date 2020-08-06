﻿using AutoMapper;
using DistanceCalculator.API.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceCalculator.API
{
    public static class Registration
    {
        public static IMvcBuilder RegisterApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Profile, AutoMapperProfile>();
            
            return services.AddMvc(config =>
            {
                config.Filters.Add<KnownExceptionFilter>();
                config.Filters.Add<OperationCancelledExceptionFilter>();
            });
        }
    }
}