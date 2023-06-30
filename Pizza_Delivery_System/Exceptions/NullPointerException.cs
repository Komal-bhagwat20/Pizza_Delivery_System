using System.Runtime.Serialization;

namespace Pizza_Delivery_System.Exceptions
{
    [Serializable]
    internal class NullPointerException : Exception
    {
        public NullPointerException()
        {
        }

        public NullPointerException(string? message) : base(message)
        {
        }

        public NullPointerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NullPointerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}