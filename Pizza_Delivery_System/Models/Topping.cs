using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Pizza_Delivery_System.Models
{
    public class Topping
    {
        [BsonId]
        public string Topping_Id { get; set; }

        [Required]
        public string Topping_Name { get; set; }
    }
}
