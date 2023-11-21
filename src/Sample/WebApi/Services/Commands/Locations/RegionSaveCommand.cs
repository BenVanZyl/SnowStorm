using MediatR;
using SnowStorm.DataContext;
using WebApi.Services.Domain;
using WebApi.Services.Queries.Locations;
using WebApi.Shared;
using WebApi.Shared.Dto;
using WebApi.Shared.Dto.Regions;

namespace WebApi.Services.Commands.Locations
{
    public class RegionSaveCommand : IRequest<CommandResultDto>
    {
        public RegionDto Data { get; set; }

        public RegionSaveCommand(RegionDto data)
        {
            Data = data;
        }
    }

    //public record RegionSaveCommand(RegionDto Data) : IRequest<CommandResultDto>;

    public class RegionSaveCommandHandler : IRequestHandler<RegionSaveCommand, CommandResultDto>
    {
        private readonly AppDbContext _dataContext;

        public RegionSaveCommandHandler(AppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<CommandResultDto> Handle(RegionSaveCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResultDto();
            try
            {
                var value = await _dataContext.Get(new GetRegionQuery(request.Data.Id));
                if (value == null)
                {
                    value = await Region.Create(request.Data, false);
                    result.Message = Messages.SuccessRecordCreated;
                }
                else
                {
                    value.Save(request.Data);
                    result.Message = Messages.SuccessRecordUpdated;
                }
                await _dataContext.Save();

                result.Success = true;
                result.Id = value.Id.ToString();

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;            
            }
            return result;            
        }
    }
}
