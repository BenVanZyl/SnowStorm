using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace SnowStorm.Infrastructure.Configurations
{
    public static class Setup
    {
        public static void All(ref IServiceCollection services, Assembly startup, Profile mappingProfile)
        {
            if (startup is null)
                throw new ArgumentNullException(nameof(startup));

            if (mappingProfile == null)
                throw new ArgumentNullException(nameof(mappingProfile));

            //setup query executors
            QueryExecutorConfiguration.Configure(ref services);

            //Setup MediatR
            MediatorConfiguration.Configure(services, startup);

            //setup automapper
            AutoMapperConfiguration.Configure(ref services, mappingProfile);
        }
    }
}
