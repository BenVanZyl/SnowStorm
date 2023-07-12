using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Shared.Dto
{
    public class CommandResultDto
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

        public int IdToInt
        {
            get
            {
                int.TryParse(Id, out var result);
                return result;
            }
        }

        public long IdToLong
        {
            get
            {
                long.TryParse(Id, out var result);
                return result;
            }
        }
    }
}
