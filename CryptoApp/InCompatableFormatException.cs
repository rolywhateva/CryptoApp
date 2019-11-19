using System;
using System.Runtime.Serialization;

namespace CryptoApp
{
    [Serializable]
    internal class InCompatibleFormatException : Exception
    {
        public InCompatibleFormatException()
        {
        }

        public InCompatibleFormatException(string message) : base(message)
        {
        }

        public InCompatibleFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InCompatibleFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}