using System.ComponentModel.DataAnnotations;

namespace Pizza_Delivery_System.Models
{
    public class Manager
    {
        [Key]
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
