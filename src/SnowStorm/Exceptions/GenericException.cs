using System;
using System.Runtime.Serialization;

namespace SnowStorm.Exceptions
{
    [Serializable]
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