using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RoomBookingAPI.Models;

namespace RoomBookingAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }

    public class JwtService : IJwtService
    {
        public string GenerateToken(User user)
        {
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "YourSuperSecretKeyForJWTTokenGeneration123456789";
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "RoomBookingAPI";
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "RoomBookingClient";
            var expirationInMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES") ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
