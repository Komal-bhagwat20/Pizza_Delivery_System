using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pizza_Delivery_System.Models
{
    public class UserDetails
    {
        [BsonId]
        public string User_Id { get; set; }
        public string Email { get; set; }
        public List<OrderDetails> OrdersList { get; set; } = new List<OrderDetails>();
    }
}
