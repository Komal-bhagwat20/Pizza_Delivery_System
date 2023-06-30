using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Pizza_Delivery_System.Models
{
    public class Orders
    {
        [Required]
        public string Pizza_Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Sizes Size { get; set; }

        [Required]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Crust Crust { get; set; }

        [Required]
        public List<string> Topping_Id { get; set; }
    }
}
