using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnowStorm
{
    public static class Extensions
    {
        public static void AddSnowStorm(this IServiceCollection services, Assembly startup, Profile mappingProfile, string connectionString, string externalAssemblyName = "")
        {
            Setup.All(ref services, startup, mappingProfile, connectionString, externalAssemblyName);
        }
    }
}
