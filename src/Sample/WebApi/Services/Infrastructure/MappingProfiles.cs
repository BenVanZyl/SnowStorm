using AutoMapper;
using WebApi.Services.Domain;
using WebApi.Shared.Dto;
using WebApi.Shared.Dto.Regions;

namespace WebApi.Services.Infrastructure
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();
                       


        }
    }
}
