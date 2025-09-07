using MediatR;
using SnowStorm;
using WebSample.SnowStorm.Server.Services.Domain;
using WebSample.SnowStorm.Server.Services.Queries;
using WebSample.SnowStorm.Shared.Dtos;

namespace WebSample.SnowStorm.Server.Services.Commands
{
    public class AddWeatherReportCommand : IRequest<long>
    {
        public WeatherReportDto Data { get; set; }

        public AddWeatherReportCommand(WeatherReportDto data)
        {
            Data = data;
        }
    }

    public class AddWeatherReportCommandHandler : IRequestHandler<AddWeatherReportCommand, long>
    {
        private readonly DataContext _dataContext;
        private readonly IMediator _mediator;
        private readonly ILogger<AddWeatherReportCommandHandler> _logger;
        private readonly QueryRunner _queries;

        public AddWeatherReportCommandHandler(DataContext dataContext, QueryRunner queries, IMediator mediator, ILogger<AddWeatherReportCommandHandler> logger)
        {
            _dataContext = dataContext;
            _queries = queries;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<long> Handle(AddWeatherReportCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{request.Data.ReportName}; {request.Data.WeatherData?.Length}; {request.Data.WeatherData};  ");

            if (string.IsNullOrEmpty(request.Data.ReportName))
                throw new NullReferenceException("Missing report name");

            if (request.Data.WeatherData == null || request.Data.WeatherData.Length == 0)
                throw new NullReferenceException("Missing report date");

            try
            {
                var value = await _queries.Get(new GetWeatherReportQuery(request.Data.ReportName));

                //await _db.Database.BeginTransactionAsync();

                if (value == null)
                    value = await WeatherReport.Create(_queries, request.Data.ReportName);
                else
                    throw new InvalidDataException($"Report name already exists: {request.Data.ReportName}");

                var dataAdded = await _mediator.Send(new AddWeatherDataCommand(request.Data.WeatherData).WithReportId(value.Id));
                
                _queries.Save();

           
                return value.Id;
            }
            catch (Exception ex)
            {
                //await _db.Database.RollbackTransactionAsync(cancellationToken);
                _logger.LogError(ex, $"Save Failed. {ex.Message}");
                throw new InvalidDataException("Failed to save Weather Data.");
            }
        }
    }
}
