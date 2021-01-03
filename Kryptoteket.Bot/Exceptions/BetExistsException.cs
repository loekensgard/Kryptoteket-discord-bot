using System;
using System.Runtime.Serialization;

namespace Kryptoteket.Bot.Exceptions
{
    public class BetExistsException : Exception
    {
        public BetExistsException()
        {
        }

        public BetExistsException(string message) : base(message)
        {
        }

        public BetExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BetExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
