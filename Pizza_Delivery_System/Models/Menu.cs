using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pizza_Delivery_System.Models
{
    public class Menu
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<Pizza> Pizzas { get; set; }
        public List<Topping> Toppings { get; set; }

        [BsonRepresentation(BsonType.String)]
        public List<Crust> Crusts { get; set; }

        [BsonRepresentation(BsonType.String)]
        public List<Sizes> Sizes { get; set; }

        public int Tax { get; set; }
    }
}
