using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LuckyoneApi.Helper
{
    public class TokenHelper
    {
        public readonly IConfiguration _configuration;

        public TokenHelper(IConfiguration configuration) 
        {
            _configuration = configuration; 
        }
        public string GenerateJwtToken(int userId, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
               new Claim("role", role),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expireTime = double.Parse( _configuration["Jwt:ExpiryInHours"] );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expireTime),
                signingCredentials: credentials

                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
