using MediatR;
using SnowStorm.DataContext;
using WebApi.Services.Queries.Locations;
using WebApi.Shared;
using WebApi.Shared.Dto;
using WebApi.Shared.Dto.Locations;

namespace WebApi.Services.Commands.Locations
{
    public class RegionPatchCommand : IRequest<CommandResultDto>
    {
        public RegionPatchDto Data { get; set; }

        public RegionPatchCommand(RegionPatchDto data)
        {
            Data = data;
        }
    }

    public class RegionPatchCommandHandler : IRequestHandler<RegionPatchCommand, CommandResultDto>
    {
        private readonly AppDbContext _dataContext;

        public RegionPatchCommandHandler(AppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<CommandResultDto> Handle(RegionPatchCommand request, CancellationToken cancellationToken)
        {
            var result = new CommandResultDto();
            try
            {
                var value = await _dataContext.Get(new GetRegionQuery(request.Data.Id));
                if (value == null)
                {
                    result.Success = false;
                    result.Id = request.Data.Id.ToString();
                    result.Message = Messages.NotFound;
                    return result;
                }

                switch (request.Data.FieldName)
                {
                    case RegionPatchDto.FieldNames.Id:
                        //PK Cannot patch.
                        break;
                    case RegionPatchDto.FieldNames.RegionDescription:
                        value.SetRegionDescription(request.Data.Value);
                        break;
                    default:
                        break;
                }

                result.Message = Messages.SuccessRecordUpdated;
                
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

