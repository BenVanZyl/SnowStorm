using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace SnowStorm.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void Configure(ref IServiceCollection services)
        {
            // Auto Mapper Configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Depreciated
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mappingProfile"></param>
        public static void Configure(ref IServiceCollection services, Profile mappingProfile)
        {
            // Auto Mapper Configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
