using Pizza_Delivery_System.Models;

namespace Pizza_Delivery_System.InterFaces
{
    public interface IManagerServices
    {
        public string LoginManager(string username, string password);
        public List<OrderDetails> ViewAllOrders();
        public void ManageOrder(string order_id, string orderStatus);
        public OrderDetails OrderDetails(string order_id);
    }
}