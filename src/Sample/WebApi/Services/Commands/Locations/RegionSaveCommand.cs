using MediatR;
using SnowStorm.Domain;
using SnowStorm.QueryExecutors;
using WebApi.Services.Domain;
using WebApi.Services.Queries.Locations;
using WebApi.Shared.Dto;

namespace WebApi.Services.Commands.Locations
{
    public class RegionSaveCommand : IRequest<bool>
    {
        public RegionDto Data { get; set; }

        public RegionSaveCommand(RegionDto data)
        {
            Data = data;
        }
    }

    public class RegionSaveCommandHandler : IRequestHandler<RegionSaveCommand, bool>
    {
        private readonly AppDbContext _dataContext;

        public RegionSaveCommandHandler(AppDbContext datacontext)
        {
            _dataContext = datacontext;
        }

        public async Task<bool> Handle(RegionSaveCommand request, CancellationToken cancellationToken)
        {
            var value = await _dataContext.Get(new GetRegionQuery(request.Data.Id));
            if (value == null)
                value = await Region.Create(request.Data);
            else
                value.Save(request.Data);

            await _dataContext.Save();

            return true;

            //throw new ThisAppExecption(StatusCodes.Status400BadRequest, "Record not found."



        }
    }
}
