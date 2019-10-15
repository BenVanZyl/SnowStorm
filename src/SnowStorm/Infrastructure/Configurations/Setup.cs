using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Reflection;

namespace SnowStorm.Infrastructure.Configurations
{
    public static class Setup
    {
        public static void All(ref IServiceCollection services, Assembly startup, Profile mappingProfile, Info info)
        {
            WithOutSwagger(ref services, startup, mappingProfile);

            // setup swagger
            SwaggerConfiguration.Configure(ref services, info);

        }

        public static void WithOutSwagger(ref IServiceCollection services, Assembly startup, Profile mappingProfile)
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
