using Microsoft.EntityFrameworkCore;
using Pizza_Delivery_System.Models;
using System.Collections.Generic;

namespace Pizza_Delivery_System.DBContext
{
    public class DataContext : DbContext
    {
        // Constructor of class 
        public DataContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        // create table of user 
        public DbSet<User> Users { get; set; }
        // create table of manager 
        public DbSet<Manager> Managers { get; set; }
    }
}
