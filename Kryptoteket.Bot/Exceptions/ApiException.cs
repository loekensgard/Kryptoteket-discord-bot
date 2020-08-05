using System;
using System.Runtime.Serialization;

namespace Kryptoteket.Bot.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException()
        {
        }

        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public int StatusCode { get; set; }
        public string Content { get; set; }
    }
}
