using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kryptoteket.Bot.Exceptions
{
    public class NBXTradesNullException : Exception
    {
        public NBXTradesNullException()
        {
        }

        public NBXTradesNullException(string message) : base(message)
        {
        }

        public NBXTradesNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NBXTradesNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
