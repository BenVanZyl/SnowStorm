using AutoMapper;
using WebSample.SnowStorm.Server.Services.Domain;
using WebSample.SnowStorm.Shared.Dtos;

namespace WebSample.SnowStorm.Server.Services.Infrastructure
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<WeatherReport, WeatherReportDto>().ReverseMap();

            CreateMap<WeatherData, WeatherDataDto>().ReverseMap();

        }
    }
}
