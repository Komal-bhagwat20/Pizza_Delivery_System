using System.Runtime.Serialization;

namespace Pizza_Delivery_System.Exceptions
{
    [Serializable]
    internal class ItemNotExistException : Exception
    {
        public ItemNotExistException()
        {
        }

        public ItemNotExistException(string? message) : base(message)
        {
        }

        public ItemNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ItemNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}