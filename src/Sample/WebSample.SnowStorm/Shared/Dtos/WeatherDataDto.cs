using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSample.SnowStorm.Shared.Dtos
{
    public class WeatherDataDto
    {
        public long? Id { get; set; }
        public long? ReportId { get; set; }
        public DateTime ForecastDate { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    }
}
