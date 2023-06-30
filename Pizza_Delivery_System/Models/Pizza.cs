using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Pizza_Delivery_System.Models
{
    public class Pizza
    {
        [BsonId]
        public string Pizza_Id { get; set; }
        [Required]
        public string Pizza_Name { get; set; }
    }
}
