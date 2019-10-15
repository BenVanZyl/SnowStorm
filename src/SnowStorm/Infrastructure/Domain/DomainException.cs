
using System;
using System.Net;

namespace SnowStorm.Infrastructure.Domain
{
    /// <summary>
    /// Default status code is 422 - UnprocessableEntity which will represent a domain exception
    /// Anything else will return a staus code 500 - Internal Server Error
    /// </summary>
    public class DomainException : Exception
    {
        public string LogMessageTemplate { get; }
        public object[] LogPropertyValues { get; }

        //default status code: 422 Unprocessable Entity
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Default status code is 422 - UnprocessableEntity which will represent a domain exception
        /// </summary>
        public DomainException(string message) : base(message)
        {
            StatusCode = (HttpStatusCode)422;
        }

        /// <summary>
        /// Throw a domain exception with additional log context
        /// Default status code is 422 - UnprocessableEntity which will represent a domain exception
        /// </summary>
        public DomainException(string message, string logMessageTemplate, params object[] logPropertyValues) : base(message)
        {
            StatusCode = (HttpStatusCode)422;
            LogMessageTemplate = logMessageTemplate;
            LogPropertyValues = logPropertyValues;
        }
    }
}
