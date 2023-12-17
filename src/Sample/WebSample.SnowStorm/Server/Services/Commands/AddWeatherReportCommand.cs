using MediatR;
using SnowStorm.DataContext;
using WebSample.SnowStorm.Server.Services.Domain;
using WebSample.SnowStorm.Server.Services.Queries;
using WebSample.SnowStorm.Shared.Dtos;

namespace WebSample.SnowStorm.Server.Services.Commands
{
    public class AddWeatherReportCommand : IRequest<bool>
    {
        public WeatherReportDto Data { get; set; }

        public AddWeatherReportCommand(WeatherReportDto data)
        {
            Data = data;
        }
    }

    public class AddWeatherReportCommandHandler : IRequestHandler<AddWeatherReportCommand, bool>
    {
        private readonly AppDbContext _dataContext;

        public AddWeatherReportCommandHandler(AppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> Handle(AddWeatherReportCommand request, CancellationToken cancellationToken)
        {
            var value = await _dataContext.Get(new GetWeatherReportQuery(request.Data.Description));

            if (value == null)
                value = await WeatherReport.Create(request.Data);
            else
                throw new InvalidDataException($"Report name already exists: {request.Data.Description}");
            
            return true;
        }
    }
}
