using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace SnowStorm
{
    public static class Converter
    {
        public static IMapper Mapper { get; set; }

        public static TDto Convert<TDto>(object source)
        {
            var result = Mapper.Map<TDto>(source);
            return result;
        }

        public static List<TDto> ConvertList<TDto>(IEnumerable<object> source)
        {
            return source.Select(item => Mapper.Map<TDto>(item)).ToList();
        }
    }
}