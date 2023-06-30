using Pizza_Delivery_System.Models;

namespace Pizza_Delivery_System.InterFaces
{
    public interface IMongoRepo
    {
        public void SeedDataInMenu();
        public IEnumerable<Menu> GetMenu();
        public void AddUser(UserDetails user);
        public UserDetails DetailsOfUser(string User_Id);
        public void AddOrder(UserDetails user);
        public List<UserDetails> ViewOrders();

    }
}