using System;
using System.Runtime.Serialization;

namespace Assets.Map.Pathing
{
    [Serializable]
    internal class InvalidPathException : Exception
    {
        public PathRequest PathRequest;

        public InvalidPathException()
        {
        }

        public InvalidPathException(PathRequest pathRequest)
        {
            PathRequest = pathRequest;
        }

        public InvalidPathException(string message) : base(message)
        {
        }

        public InvalidPathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}