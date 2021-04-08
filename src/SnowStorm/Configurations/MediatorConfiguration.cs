using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SnowStorm.Configurations
{
    public static class MediatorConfiguration
    {
        public static void Configure(IServiceCollection services, Assembly startup, string externalAssemblyName = "")
        {
            //services.AddMediatR(typeof(startup).GetTypeInfo().Assembly);
            services.AddMediatR(startup);

            if (!string.IsNullOrWhiteSpace(externalAssemblyName))
                services.AddMediatR(Assembly.Load(externalAssemblyName));
        }
    }
}
/// Notes
/// https://ovaismehboob.com/2018/01/31/implementing-mediator-pattern-in-net-core-using-mediatr/
/// https://github.com/jbogard/MediatR/wiki
/// https://github.com/jbogard/MediatR.Extensions.Microsoft.DependencyInjection
///
/// Install-Package MediatR
///

