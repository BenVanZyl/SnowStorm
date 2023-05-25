using AutoMapper;
using WebApi.Services.Domain;
using WebApi.Shared.Dto;

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
