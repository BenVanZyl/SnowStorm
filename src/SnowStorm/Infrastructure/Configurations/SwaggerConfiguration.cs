using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace SnowStorm.Infrastructure.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void Configure(ref IServiceCollection services, Info info) =>
            services.AddSwaggerGen(c => c.SwaggerDoc(info.Version, info));
    }
}
