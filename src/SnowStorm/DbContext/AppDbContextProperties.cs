using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SnowStorm.QueryExecutors;

namespace SnowStorm.Domain
{
    public partial class AppDbContext
    {
        private IQueryableProvider _queryableProvider;
        public IQueryableProvider QueryableProvider
        {
            get
            {
                if (_queryableProvider == null)
                {
                    var serviceProvider = Container.Instance;
                    _queryableProvider = serviceProvider.GetService<IQueryableProvider>();
                }
                return _queryableProvider;
            }
        }

        private IMapper _mapper;
        public IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    var serviceProvider = Container.Instance;
                    _mapper = serviceProvider.GetService<IMapper>();
                }
                return _mapper;
            }
        }

        private ILogger<AppDbContext> _logger = null;
        public ILogger<AppDbContext> Logger
        {
            get
            {
                if (_logger == null)
                {
                    var serviceProvider = Container.Instance;
                    _logger = serviceProvider.GetService<ILogger<AppDbContext>>();
                }
                return _logger;
            }
        }

    }
}
