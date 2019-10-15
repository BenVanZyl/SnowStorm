using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace SnowStorm.Infrastructure
{
    public static class Projector
    {
        public static Mapper Mapper { get; set; }

        public static TDto Project<TDto>(object source)
        {
            var result = Mapper.Map<TDto>(source);
            return result;
        }

        public static List<TDto> ProjectList<TDto>(IEnumerable<object> source)
        {
            return source.Select(item => Mapper.Map<TDto>(item)).ToList();
        }
    }
}
