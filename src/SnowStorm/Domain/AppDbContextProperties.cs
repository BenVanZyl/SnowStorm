using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SnowStorm.QueryExecutors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowStorm.Domain
{
    public partial class AppDbContext
    {
        public IQueryableProvider QueryableProvider { get; set; }

        private IMapper mapper;
        public IMapper Mapper
        {
            get
            {
                if (mapper == null)
                {
                    var serviceProvider = Container.Instance;
                    mapper = serviceProvider.GetService<IMapper>();
                }
                return mapper;
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
