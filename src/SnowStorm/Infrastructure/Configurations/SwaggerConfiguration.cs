using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace SnowStorm.Infrastructure.Configurations
{
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Swagger confifuration steps for 
        /// </summary>
        /// <param name="services">Microsoft.Extensions.DependencyInjection.IServiceCollection</param>
        /// <param name="info">Swashbuckle.AspNetCore.Swagger.Info</param>
        public static void Configure(ref IServiceCollection services, Info info) =>
            services.AddSwaggerGen(c => c.SwaggerDoc(info.Version, info));

        /// <summary>
        /// Swagger confifuration steps for 
        /// </summary>
        /// <param name="app">Microsoft.AspNetCore.Builder.IApplicationBuilder</param>
        /// <param name="name">"Api Name - Whatever whenever"</param>
        /// <param name="url">"/swagger/v1/swagger.json"</param>
        /// <param name="routePrefix">"swagger"</param>
        public static void Configure(ref IApplicationBuilder app, string name, string url = "/swagger/v1/swagger.json",  string routePrefix = "swagger")
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url, name);
                c.RoutePrefix = routePrefix;
            });
        }

    }
}
