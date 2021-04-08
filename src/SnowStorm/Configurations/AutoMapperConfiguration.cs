using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace SnowStorm.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void Configure(ref IServiceCollection services, Profile mappingProfile)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(mappingProfile);
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
