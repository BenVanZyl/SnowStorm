using MediatR;
using SnowStorm.DataContext;
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
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;
        private readonly ILogger<AddWeatherReportCommandHandler> _logger;

        public AddWeatherReportCommandHandler(AppDbContext dataContext, IMediator mediator, ILogger<AddWeatherReportCommandHandler> logger)
        {
            _db = dataContext;
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
                var value = await _db.Get(new GetWeatherReportQuery(request.Data.ReportName));

                //await _db.Database.BeginTransactionAsync();

                if (value == null)
                    value = await WeatherReport.Create(request.Data.ReportName);
                else
                    throw new InvalidDataException($"Report name already exists: {request.Data.ReportName}");

                var dataAdded = await _mediator.Send(new AddWeatherDataCommand(request.Data.WeatherData).WithReportId(value.Id));
                
                await _db.Save();

                //await _db.Database.CommitTransactionAsync(cancellationToken);
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
