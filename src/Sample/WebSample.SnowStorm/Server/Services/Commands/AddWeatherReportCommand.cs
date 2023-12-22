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
        private readonly IMediator _mediator;

        public AddWeatherReportCommandHandler(AppDbContext dataContext, IMediator mediator)
        {
            _dataContext = dataContext;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddWeatherReportCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Data.ReportName))
                throw new NullReferenceException("Missing report name");

            if (request.Data.WeatherData == null || request.Data.WeatherData.Length == 0)
                throw new NullReferenceException("Missing report date");

            var value = await _dataContext.Get(new GetWeatherReportQuery(request.Data.ReportName));

            if (value == null)
                value = await WeatherReport.Create(request.Data);
            else
                throw new InvalidDataException($"Report name already exists: {request.Data.ReportName}");

            var dataAdded = await _mediator.Send(new AddWeatherDataCommand(request.Data.WeatherData).WithReportId(value.Id));

            return true;
        }
    }
}
