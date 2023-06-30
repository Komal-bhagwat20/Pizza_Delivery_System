using Pizza_Delivery_System.DBContext;
using Pizza_Delivery_System.Exceptions;
using Pizza_Delivery_System.InterFaces;
using Pizza_Delivery_System.Models;

namespace Pizza_Delivery_System.Services
{
    public class UserServices: IUserServices
    {
        //Creating Objects
        private readonly DataContext _DbContext;
        private readonly IMongoRepo mongoRepo;

        private readonly IJwtToken jwtToken;
        private static string UserId;

        public UserServices(DataContext _DbContext, IJwtToken jwtToken, IMongoRepo mongoRepo)
        {
            this._DbContext = _DbContext;
            this.jwtToken = jwtToken;
            this.mongoRepo = mongoRepo;
        }
        //Registration Method
        public bool registerUser(User user)
        {
            //Check user already exist or not.
            var customer = _DbContext.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            if (customer != null)
            {
                //User Alredy Register
                throw new UserAlreadyExistsException("User Alredy Exist With Given Email: " + user.Email);
            }
            var newUser = new User()
            {
                User_Id = "k" + TimeOnly.FromDateTime(DateTime.UtcNow).ToString(), 
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                ConfirmPassword = user.ConfirmPassword
            };
            _DbContext.Users.Add(newUser);
            var updateLine = _DbContext.SaveChanges();

            // Adding Details of user to MongoDb 
            mongoRepo.AddUser(new UserDetails()
            {
                User_Id = newUser.User_Id,
                Email = newUser.Email,
                OrdersList = new List<OrderDetails>()
            });
            //Checking User Register or not
            if (updateLine > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Login Method
        public string LoginUser(string email, string password)
        {
            //Finding user in DB
            var customer = _DbContext.Users.FirstOrDefault(x => x.Email == email);

            if (customer == null)
            {
                //User not present in Db
                throw new UserNotFoundException("User Not Found With Given Email: " + email);
            }
            else if (email == customer.Email && password == customer.Password)
            {
                // store the user id to global variable 
                UserId = customer.User_Id;

                //Returning Jwt Token 
                return jwtToken.CreateJwtToken(customer, "user");
            }
            else
            {
                throw new IncorrectEmailOrPasswordException("Incorrect Email or Password");
            }


        }
        //Forgot Password
        public string ForgetPassword(string Email)
        {
            //Finding user in DB
            var customer = _DbContext.Users.FirstOrDefault(x => x.Email == Email);

            if (customer != null)
            {
                //Fetch Password
                return customer.Password;
            }
            else
            {
                throw new UserNotFoundException("user dopesn't exist");
            }
        }
        public IEnumerable<Menu> ViewMenu()
        {
            //Fetching menu from MongoDb Database 
            return mongoRepo.GetMenu();
        }
        public string CreateOrder(Orders orders)
        {
            //Fetching users Details
            UserDetails user = mongoRepo.DetailsOfUser(UserId);

            //Checking User is present or not 
            if(user == null)
            {
                throw new NullPointerException("Object reference is null");
            }

            //Fetching menu
            var menu = mongoRepo.GetMenu();
            if (menu == null)
            {
                return "Sorry for Inconvinience ";
            }

            // Checking if Pizza Available or not
            bool hasPizza = menu.Any(items => items.Pizzas.Any(p => p.Pizza_Id == orders.Pizza_Id));
            
            //If Pizza Id does not present
            if (hasPizza == false ) 
            {
                return "Pizza or Topping with given Id does not present";
            }

            //Else

            //Calculating Price for 1 quantity
            var Amt = (orders.Size == Sizes.Small ? 89 : (orders.Size == Sizes.Medium ? 120  : 169)) + (orders.Topping_Id.Count * 50);

            //Creating object of OrderDetails Model 
            var orderDetails = new OrderDetails()
            {
                Order_Id = "K" + TimeOnly.FromDateTime(DateTime.Now).ToString("s"),
                OrderPizzaDetails = orders,
                OrderDate = DateTime.Now,
                OrderStatus = "Accepted",
                OrderAmount = Amt,
                OrderTax = 20,
                OrderSubTotal = Amt + 20
            };

            //Rejecting if quantity is 0 or less
            if (orders.Quantity <= 0)
            {
                return "Please Enter Quantity......";
            }
            var TotalAmt = Amt * orders.Quantity + 20;

            //Adding order Details in UserDetails Model
            user.OrdersList.Add(orderDetails);

            //Add order in users ordersList
            mongoRepo.AddOrder(user);

            
            return "Order Accepted Successfully....."+ "Your Bill :" + TotalAmt ;
        }
        public List<OrderDetails> TrackOrder()
        {
            // Create object of UserDetails class to fetch Details of user
            UserDetails user = mongoRepo.DetailsOfUser(UserId);
            var orderDetails = new List<OrderDetails>();
            foreach (var item in user.OrdersList)
            {
                //Cheching Order status 
                if (item.OrderStatus != "Delivered")
                {
                    orderDetails.Add(item);
                }
            }
            return orderDetails;
        }
        public List<OrderDetails> OrderHistory()
        {
            // Create object of UserDetails class to fetch Details of user
            UserDetails user = mongoRepo.DetailsOfUser(UserId);

            // Creating Object of OrderDetails Class to get details of order
            var orderDetailsList = new List<OrderDetails>();

            // Looping in user orders list 
            foreach (var item in user.OrdersList)
            {
                orderDetailsList.Add(item);
            }

            // Return List of orders of user 
            return orderDetailsList;
        }
    }
}
