using System.Runtime.Serialization;

namespace Pizza_Delivery_System.Exceptions
{
    [Serializable]
    internal class OrderNotFound : Exception
    {
        public OrderNotFound()
        {
        }

        public OrderNotFound(string? message) : base(message)
        {
        }

        public OrderNotFound(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}