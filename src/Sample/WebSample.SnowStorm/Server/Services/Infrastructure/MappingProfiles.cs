using AutoMapper;
using WebSample.SnowStorm.Server.Services.Domain;
using WebSample.SnowStorm.Shared.Dtos;

namespace WebSample.SnowStorm.Server.Services.Infrastructure
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<WeatherReport, WeatherReportDto>()
                .ForMember(d => d.WeatherData, m => m.Ignore());

            CreateMap<WeatherData, WeatherDataDto>();

        }
    }
}
