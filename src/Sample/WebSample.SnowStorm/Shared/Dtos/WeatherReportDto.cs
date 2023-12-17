using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSample.SnowStorm.Shared.Dtos
{
    public class WeatherReportDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        
        public WeatherForecast[]? WeatherData { get; set; }

        //public List<WeatherDataDto> WeatherData { get; set; }

    }
}
