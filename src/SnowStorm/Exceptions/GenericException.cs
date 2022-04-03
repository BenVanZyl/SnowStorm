using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SnowStorm.Exceptions
{
    public class GenericException : Exception
    {
        public GenericException()
        {
        }

        public GenericException(string message) : base(message)
        {
        }

        public GenericException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GenericException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }

   
}
