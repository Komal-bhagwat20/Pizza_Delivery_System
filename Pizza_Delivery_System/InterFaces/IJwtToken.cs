using Pizza_Delivery_System.Models;

namespace Pizza_Delivery_System.InterFaces
{
    public interface IJwtToken
    {
        
        string CreateJwtToken(User user, string role);
        string CreateJwtTokenManager(Manager customer, string role);
    }
}