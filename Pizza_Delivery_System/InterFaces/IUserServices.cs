using Pizza_Delivery_System.Models;

namespace Pizza_Delivery_System.InterFaces
{
    public interface IUserServices
    {
        public bool registerUser(User user);
        public string LoginUser(string email, string password);
        ///*public void logout();*/
        public string ForgetPassword(string email);
        public IEnumerable<Menu> ViewMenu();
        public string CreateOrder(Orders orders);
        public List<OrderDetails> TrackOrder();
        public List<OrderDetails> OrderHistory();
    }
}