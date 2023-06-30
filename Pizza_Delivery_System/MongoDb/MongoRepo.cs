using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Pizza_Delivery_System.InterFaces;
using Pizza_Delivery_System.Models;

namespace Pizza_Delivery_System.MongoDb
{
    public class MongoRepo : IMongoRepo
    {
        private readonly IMongoCollection<UserDetails> UsersCollection;
        private readonly IMongoCollection<Menu> MenuCollection;
        private readonly IOptions<MongoSetting> mgSetting;
        public MongoRepo(IOptions<MongoSetting> mgSetting)
        {
            this.mgSetting = mgSetting;
            var mongoClient = new MongoClient(mgSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mgSetting.Value.DatabaseName);
            UsersCollection = mongoDatabase.GetCollection<UserDetails>(mgSetting.Value.userCollection);
            MenuCollection = mongoDatabase.GetCollection<Menu>(mgSetting.Value.menuListName);
        }

        //For seeding data of menu in mongodb databse.
        public void SeedDataInMenu()
        {
            // Clear existing data (optional)
            MenuCollection.DeleteMany(new BsonDocument());

            // Insert new data
            var menu = new Menu
            {
                Pizzas = new List<Pizza>
                {
                    new Pizza { Pizza_Id = "P01", Pizza_Name = "Malai Chicken Tikka" },
                    new Pizza { Pizza_Id = "P02", Pizza_Name = "Cheese Overload" },
                    new Pizza { Pizza_Id = "P03", Pizza_Name = "Grilled Cheese" },
                    new Pizza { Pizza_Id = "P04", Pizza_Name = "Ckicken Keema" },
                    new Pizza { Pizza_Id = "P05", Pizza_Name = "Panner Onion with Desi Makhani Sauce" },
                    new Pizza { Pizza_Id = "P06", Pizza_Name = "Paneer" },
                    new Pizza { Pizza_Id = "P07", Pizza_Name = "Chicken Sausage" },
                    new Pizza { Pizza_Id = "P08", Pizza_Name = "Blazing Onion & Paprika" },
                    new Pizza { Pizza_Id = "P09", Pizza_Name = "Pepper barbecue Chicken" }
                },
                Toppings = new List<Topping>
                {
                    new Topping { Topping_Id = "T01", Topping_Name = "Pepperoni" },
                    new Topping { Topping_Id = "T02", Topping_Name = "Sausage" },
                    new Topping { Topping_Id = "T03", Topping_Name = "Ckicken" },
                    new Topping { Topping_Id = "T04", Topping_Name = "Onions (red or white)" },
                    new Topping { Topping_Id = "T05", Topping_Name = "Tomatoes (fresh or sun-dried)" },
                    new Topping { Topping_Id = "T06", Topping_Name = "Olives (black or green)" },
                    new Topping { Topping_Id = "T07", Topping_Name = "Fresh cilantro" },
                    new Topping { Topping_Id = "T08", Topping_Name = "Mozzarella cheese" }
                },
                Crusts = new List<Crust>
                {
                    Crust.StuffedCrust,
                    Crust.CrackerCrust,
                    Crust.FlatBreadCrust,
                    Crust.ThinCrust,
                    Crust.ThickCrust,
                    Crust.CrispyCrust

                },
                Sizes = new List<Sizes>
                {
                    Sizes.Small,
                    Sizes.Medium,
                    Sizes.Large
                },
                Tax = 20
            };
            MenuCollection.InsertOne(menu);
        }

        // Listing Menu items 
        public IEnumerable<Menu> GetMenu() => MenuCollection.Find(_ => true).ToList();


        //Adding user DB
        public void AddUser(UserDetails user)
        {
            Console.WriteLine("Adding User: " + user.ToString());
            UsersCollection.InsertOne(user);
        }

        // Details of User
        public UserDetails DetailsOfUser(string User_Id)
        {
            return UsersCollection.Find(a => a.User_Id == User_Id).FirstOrDefault();
        }


        //Add order to orders
        public void AddOrder(UserDetails user)
        {
            UsersCollection.FindOneAndReplace(a => a.User_Id == user.User_Id, user);

        }

        //Retrive all orders present in user collection.
        public List<UserDetails> ViewOrders()
        {
            return UsersCollection.Find(new BsonDocument()).ToList();
        }



    }
}
