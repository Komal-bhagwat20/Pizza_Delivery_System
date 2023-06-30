using MongoDB.Bson.Serialization.Attributes;

namespace Pizza_Delivery_System.Models
{
    public class OrderDetails
    {
        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string Order_Id { get; set; }
        public Orders OrderPizzaDetails { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; }

        public int OrderAmount { get; set; }

        public int OrderTax { get; set; }

        public int OrderSubTotal { get; set; }
    }
}
