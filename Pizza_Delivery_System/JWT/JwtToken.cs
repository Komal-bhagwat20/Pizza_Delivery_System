using Microsoft.IdentityModel.Tokens;
using Pizza_Delivery_System.InterFaces;
using Pizza_Delivery_System.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pizza_Delivery_System.JWT
{
    public class JwtToken : IJwtToken
    {
        private readonly IConfiguration _config;

        public JwtToken(IConfiguration configuration)
        {
            this._config = configuration;
        }
        public string CreateJwtToken(User user, string role)
        {

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Role, role));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string CreateJwtTokenManager(Manager manager, string role)
        {
            // Create list of claims
            var claims = new List<Claim>
            {
                // Adding item to list 
                new Claim(ClaimTypes.NameIdentifier, manager.Username),
                new Claim(ClaimTypes.Role, role)
            };

            // Symmetric security Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // Sign in Credentials 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            // create Security Token 
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            // Return JWT Token Using JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
