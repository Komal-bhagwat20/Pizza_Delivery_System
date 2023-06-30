using Pizza_Delivery_System.DBContext;
using Pizza_Delivery_System.Exceptions;
using Pizza_Delivery_System.InterFaces;
using Pizza_Delivery_System.Models;

namespace Pizza_Delivery_System.Services
{
    public class ManagerServices: IManagerServices
    {

        // Objects of required file
        private readonly DataContext _DbContext;
        private readonly IJwtToken jwtToken;
        private readonly IMongoRepo mongoRepo;
        public ManagerServices(DataContext _DbContext, IJwtToken jwtToken, IMongoRepo mongoRepo)
        {
            this._DbContext = _DbContext;
            this.jwtToken = jwtToken;
            this.mongoRepo = mongoRepo;
        }

        // Login method
        public string LoginManager(string username, string password)
        {
            // fetch manager detail from DB
            var manager = _DbContext.Managers.FirstOrDefault(x => x.Username == username);

            // Check Manager present or not 
            if (manager == null)
            {
                throw new UserNotFoundException("Manager Not Found With Given Username: " + username);
            }

            // Check Credentials are true 
            else if (username == manager.Username && password == manager.Password)
            {
                return jwtToken.CreateJwtTokenManager(manager, "manager");
                //return "LoginSuccessful";
            }
            else
            {
                throw new IncorrectEmailOrPasswordException("Incorrect Username or Password");
            }


        }


        // View all orders method
        public List<OrderDetails> ViewAllOrders()
        {
            // Object of OrderDeatils models
            var orderDetails = new List<OrderDetails>();

            // Storing orders in list 
            List<UserDetails> Users = mongoRepo.ViewOrders();

            // Loop through items of orders list 
            foreach (var user in Users)
            {
                // getting details of orders of one and one user 
                foreach (var item in user.OrdersList)
                {
                    // Add details in list 
                    orderDetails.Add(item);
                }
            }
            return orderDetails;

        }

        // Method to Manage orders
        public void ManageOrder(string order_id, string orderStatus)
        {

            bool flag = false;

            // Storing all orders in list 
            List<UserDetails> Users = mongoRepo.ViewOrders();

            // Loop through list 
            foreach (var user in Users)
            {
                // Loop that fetch order details of specific user
                foreach (var item in user.OrdersList)
                {
                    // Check Order id 
                    if (item.Order_Id == order_id)
                    {
                        // store order status stated by manager 
                        item.OrderStatus = orderStatus;

                        // Add order to users order list 
                        mongoRepo.AddOrder(user);

                        flag = true;
                        return;
                    }
                }
            }
            // Check updates are store or not 
            if (!flag)
            {
                throw new OrderNotFound("Order Not Found With Given Order_Id: " + order_id);
            }
        }

        //Fetching order details
        public OrderDetails OrderDetails(string order_id)
        {
            // Objects of OrderDetails models
            OrderDetails orderDetails = null;

            // Store order details in list 
            List<UserDetails> Users = mongoRepo.ViewOrders();

            // Loop through list 
            foreach (var user in Users)
            {
                // Loop that fetch order details of specific user
                foreach (var item in user.OrdersList)
                {
                    // Check Order id 
                    if (item.Order_Id == order_id)
                    {
                        // fetch order details of given order id 
                        orderDetails = item;
                        break;
                    }
                }
            }

            if (orderDetails != null)
            {
                return orderDetails;
            }
            else
            {
                throw new OrderNotFound("Order Not Found With Given Order_Id: " + order_id);
            }
        }
    }
}
