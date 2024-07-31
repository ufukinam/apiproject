using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens; // Add this using directive
using System.Security.Claims;
using System.Text;

namespace MyProject.Application
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string userId, string email)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var claims = new Dictionary<string, object>
            {
                [ClaimTypes.Email] = email,
                [ClaimTypes.Sid] = "3c545f1c-cc1b-4cd5-985b-8666886f985b"
            };

            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                {
                    Claims = claims,
                    IssuedAt = null,
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.Now.AddMinutes(30),
                    Issuer = jwtSettings["Issuer"],
                    Audience = jwtSettings["Audience"],
                    SigningCredentials = new SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                };
            
            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
            handler.SetDefaultTimesOnTokenCreation = false;

            var tokenString = handler.CreateToken(tokenDescriptor);
            return tokenString;
        }
    }
}