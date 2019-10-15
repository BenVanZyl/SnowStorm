using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SnowStorm.Infrastructure.Configurations
{
    public static class MediatorConfiguration
    {
        public static void Configure(IServiceCollection services, Assembly startup)
        {
            //services.AddMediatR(typeof(startup).GetTypeInfo().Assembly);
            services.AddMediatR(startup);
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

