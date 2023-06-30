using System.Runtime.Serialization;

namespace Pizza_Delivery_System.Exceptions
{
    [Serializable]
    internal class IncorrectEmailOrPasswordException : Exception
    {
        public IncorrectEmailOrPasswordException()
        {
        }

        public IncorrectEmailOrPasswordException(string? message) : base(message)
        {
        }

        public IncorrectEmailOrPasswordException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected IncorrectEmailOrPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}